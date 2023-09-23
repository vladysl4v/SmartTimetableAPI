using Newtonsoft.Json;
using System.Text.RegularExpressions;

using WebTimetable.Application.Deserializers;
using WebTimetable.Application.Models;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Application.Services.Exceptions;


namespace WebTimetable.Application.Services
{
    public class DtekOutagesService : IOutagesService
    {
        private Dictionary<int, Dictionary<DayOfWeek, List<Outage>>> _outages;
        private readonly IHttpClientFactory _httpFactory;
        public DtekOutagesService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }
        public List<Outage> GetOutages(TimeOnly startTime, TimeOnly endTime, DayOfWeek dayOfWeek, int outageGroup)
        {
            return _outages[outageGroup][dayOfWeek].Where(x => IsIntervalsIntersects(startTime, endTime, x.Start, x.End)).ToList();
        }

        public async Task InitializeOutages()
        {
            string source = "https://www.dtek-kem.com.ua/ua/shutdowns";
            var httpClient = _httpFactory.CreateClient();
            try
            {
                var request = await httpClient.GetStringAsync(source);
                var serializedData = Regex.Match(request, "\"data\":{.*").Value[7..^1];
                _outages = DeserializeObject(serializedData);
            }
            catch (Exception ex)
            {
                throw new OutagesNotLoadedException(ex, source, "Error during loading/deserializing.");
            }
        }

        private Dictionary<int, Dictionary<DayOfWeek, List<Outage>>> DeserializeObject(string serializedData)
        {
            var allGroups =
                JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<Outage>>>>(serializedData,
                    new OutageFactory());

            return allGroups.ToDictionary(group => int.Parse(group.Key),
                    group => group.Value.ToDictionary(item => ConvertToDayOfWeek(item.Key),
                        item => item.Value.Where(x => x.Type != OutageType.Not).ToList()));
        }

        private DayOfWeek ConvertToDayOfWeek(string value)
        {
            var integer = int.Parse(value);
            return integer == 7 ? DayOfWeek.Sunday : (DayOfWeek)integer;
        }

        private bool IsIntervalsIntersects(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
        {
            var isStartIntersects = start2 <= start1 && start1 < end2;
            var isEndIntersects = start2 < end1 && end1 <= end2;
            var isInside = start1 <= start2 && end1 >= end2;
            return isStartIntersects || isEndIntersects || isInside;
        }
    }
}
