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
                string message = pair.Value switch
                {
                    "yes" => "Світла немає",
                    "maybe" => "Можливо відключення",
                    _ => "Світло є"
                };

                OutageType outage = pair.Value switch
                {
                    "yes" => OutageType.Definite,
                    "maybe" => OutageType.Possible,
                    _ => OutageType.Not
                };
                int time = int.Parse(pair.Key);

                if (!TimeOnly.TryParseExact(time.ToString(), "%H", out var outageEnd))
                {
                    outageEnd = new TimeOnly(23, 59);
                }
                output.Add(new Outage
                {
                    Type = outage,
                    Start = TimeOnly.ParseExact((time - 1).ToString(), "%H"),
                    End = outageEnd,
                    Text = message
                });
            }

            return output;
        }
    }
}
