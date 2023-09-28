using WebTimetable.Application.Models;


namespace WebTimetable.Application.Deserializers
{
    public class OutageFactory : FactoryConverter<List<Outage>, Dictionary<string, string>>
    {
        public override List<Outage> CreateAndPopulate(Type objectType, Dictionary<string, string> arguments)
        {
            var output = new List<Outage>();
            foreach (var pair in arguments)
            {
                bool? isDefinite = pair.Value switch
                {
                    "yes" => true,
                    "maybe" => false,
                    _ => null
                };
                int time = int.Parse(pair.Key);

                if (!TimeOnly.TryParseExact(time.ToString(), "%H", out var outageEnd))
                {
                    outageEnd = new TimeOnly(23, 59);
                }
                output.Add(new Outage
                {
                    IsDefinite = isDefinite,
                    Start = TimeOnly.ParseExact((time - 1).ToString(), "%H"),
                    End = outageEnd,
                });
            }

            return output;
        }
    }
}
