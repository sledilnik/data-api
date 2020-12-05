using SloCovidServer.Models;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class AgeDailyDeathsSloveniaMapper: Mapper
    {
        public ImmutableArray<AgeDailyDeathsSloveniaDay> GetFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var query = from l in IterateLines(lines)
                        let fields = ParseLine(l)
                        let date = GetDate(fields[dateIndex])
                        let data = ExtractData(header, fields)
                        select new AgeDailyDeathsSloveniaDay
                        (
                            date.Year,
                            date.Month,
                            date.Day, 
                            data.Male,
                            data.Female
                        );
            return query.ToImmutableArray();
        }

        public (ImmutableDictionary<string, int?> Male, ImmutableDictionary<string, int?> Female) ExtractData(ImmutableDictionary<string, int> header, 
            ImmutableArray<string> fields)
        {
            ImmutableDictionary<string, int?> male = ImmutableDictionary<string, int?>.Empty;
            ImmutableDictionary<string, int?> female = ImmutableDictionary<string, int?>.Empty;
            foreach (var pair in header)
            {
                var parts = pair.Key.Split('.');
                if (parts.Length > 2)
                {
                    int? value = GetInt(fields[pair.Value]);
                    switch (parts[2])
                    {
                        case "female":
                            female = female.Add(parts[3], value);
                            break;
                        case "male":
                            male = male.Add(parts[3], value);
                            break;
                    }
                }
            }
            return (male, female);
        }
    }
}
