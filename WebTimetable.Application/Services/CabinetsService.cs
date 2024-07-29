using System.Drawing;
using System.Net;
using Newtonsoft.Json;
using WebTimetable.Application.Exceptions;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Services.Abstractions;

namespace WebTimetable.Application.Services;

public class CabinetsService : ICabinetsService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly ICacheHandler _cache;
    private const string CabinetsListCacheKey = "CabinetsList";

    public CabinetsService(IHttpClientFactory httpFactory, ICacheHandler cacheHandler)
    {
        _httpFactory = httpFactory;
        _cache = cacheHandler;
    }

    public async Task<bool> IsCabinetExistsAsync(string number, CancellationToken token)
    {
        if (_cache.TryRetrieveCache(out List<string> cacheEntry, CabinetsListCacheKey))
        {
            return cacheEntry.Contains(number);
        }
        
        var cabinets = await GetCabinetListAsync(token);
        _cache.SetCache(cabinets, CabinetsListCacheKey);
        
        return cabinets.Contains(number);
    }

    public async Task<byte[]?> GetCabinetImageAsync(string number, CancellationToken token)
    {
        var url = $"https://graph.krok.edu.ua/api/v1/university-maps/navigation-guides/{number}";

        var httpClient = _httpFactory.CreateClient();
        try
        {
            var response = await httpClient.GetAsync(url, token);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsByteArrayAsync(token);;
        }
        catch (Exception ex) when (ex is not TaskCanceledException)
        {
            throw new InternalServiceException(ex, "Cabinet image cannot be received.",
                "Error during loading image from Graph KROK.");
        }
    }

    private async Task<List<string>> GetCabinetListAsync(CancellationToken token)
    {
        var url = "https://graph.krok.edu.ua/api/v1/university-maps/filters?lang=uk-UA";

        var httpClient = _httpFactory.CreateClient();
        try
        {
            string serializedData = await httpClient.GetStringAsync(url, token);
            var data = JsonConvert.DeserializeObject<Dictionary<string,string>>(serializedData);
            var outputCabinets = data!.Select(x => x.Value).Distinct().ToList();
            return outputCabinets;
        }
        catch (Exception ex) when (ex is not TaskCanceledException)
        {
            throw new InternalServiceException(ex, "Cabinets list cannot be received.",
                "Error during loading/deserializing data from Graph KROK.");
        }
    }
}