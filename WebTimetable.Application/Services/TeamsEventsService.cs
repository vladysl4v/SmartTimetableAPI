using Microsoft.Graph;
using Microsoft.Graph.Models;

using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services
{
    public class TeamsEventsService : IEventsService
    {
        private readonly GraphServiceClient _graphClient;
        public TeamsEventsService(GraphServiceClient graphServiceClient)
        {
            _graphClient = graphServiceClient;
        }
        public List<Event> GetEvents(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            throw new NotImplementedException();
        }
    }
}
