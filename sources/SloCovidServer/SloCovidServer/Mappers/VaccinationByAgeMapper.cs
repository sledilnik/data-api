using SloCovidServer.Models;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class VaccinationByAgeMapper: Mapper
    {
        public ImmutableArray<VaccinationByAgeDay> GetFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var query = from l in IterateLines(lines)
                        let fields = ParseLine(l)
                        let date = GetDate(fields[dateIndex])
                        let data = ExtractData(header, fields)
                        select new VaccinationByAgeDay
                        (
                            date.Year,
                            date.Month,
                            date.Day,
                            Ages: data
                        );
            return query.ToImmutableArray();
        }

        public ImmutableDictionary<string, VaccinationByAgeToDate> ExtractData(ImmutableDictionary<string, int> header, 
            ImmutableArray<string> fields)
        {
            ImmutableDictionary<string, VaccinationByAgeToDate> result = ImmutableDictionary<string, VaccinationByAgeToDate>.Empty;
            foreach (var pair in header)
            {
                var parts = pair.Key.Split('.');
                if (parts.Length == 5)
                {
                    int? value = GetInt(fields[pair.Value]);
                    if (!result.TryGetValue(parts[2], out var day))
                    {
                        day = new VaccinationByAgeToDate(null, null);
                    }
                    switch (parts[3])
                    {
                        case "1st":
                            day = day with { First = value };
                            break;
                        case "2nd":
                            day = day with { Second = value };
                            break;
                    }
                    result = result.SetItem(parts[2], day);
                }
            }
            return result;
        }
    }
}
