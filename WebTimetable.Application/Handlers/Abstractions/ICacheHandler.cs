namespace WebTimetable.Application.Handlers.Abstractions;

public interface ICacheHandler
{
    public void SetCache<T>(T value, params object[] compositeKey);
    public bool TryRetrieveCache<T>(out T value, params object[] compositeKey);
}