using Microsoft.Graph;

using WebTimetable.Application.Models;
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

        public void ConfigureEvents(IEnumerable<Lesson> schedule)
        {
            throw new NotImplementedException();
        }
    }
}
