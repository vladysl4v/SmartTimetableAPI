namespace WebTimetable.Application.Services.Abstractions;

public interface ICabinetsService
{
    public Task<bool> IsCabinetExistsAsync(string number, CancellationToken token);
    public Task<byte[]?> GetCabinetImageAsync(string number, CancellationToken token);
}