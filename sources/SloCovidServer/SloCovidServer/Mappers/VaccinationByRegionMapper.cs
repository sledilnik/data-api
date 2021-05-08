using SloCovidServer.Models;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class VaccinationByRegionMapper: Mapper
    {
        public ImmutableArray<VaccinationByRegionDay> GetFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var query = from l in IterateLines(lines)
                        let fields = ParseLine(l)
                        let date = GetDate(fields[dateIndex])
                        let data = ExtractData(header, fields)
                        select new VaccinationByRegionDay
                        (
                            date.Year,
                            date.Month,
                            date.Day,
                            Regions: data
                        );
            return query.ToImmutableArray();
        }

        public ImmutableDictionary<string, VaccinationByRegionDayData> ExtractData(ImmutableDictionary<string, int> header, 
            ImmutableArray<string> fields)
        {
            ImmutableDictionary<string, VaccinationByRegionDayData> result = ImmutableDictionary<string, VaccinationByRegionDayData>.Empty;
            foreach (var pair in header)
            {
                var parts = pair.Key.Split('.');
                if (parts.Length >= 4)
                {
                    int? value = GetInt(fields[pair.Value]);
                    if (!result.TryGetValue(parts[2], out var day))
                    {
                        day = new VaccinationByRegionDayData(null, null);
                    }
                    switch (parts[3])
                    {
                        case "1st":
                            day = day with { First = GetTodayToDate(day.First, parts, value) };
                            break;
                        case "2nd":
                            day = day with { Second = GetTodayToDate(day.Second, parts, value) };
                            break;
                    }
                    result = result.SetItem(parts[2], day);
                }
            }
            return result;
        }

        internal TodayToDate GetTodayToDate(TodayToDate source, string[] parts, int? value)
        {
            if (!value.HasValue && source is null)
            {
                return null;
            }
            source = source ?? TodayToDate.Empty;
            source = parts.Length switch
            {
                4 => source with { Today = value },
                5 => source with { ToDate = value },
                _ => source
            };
            return source;
        }
    }
}
