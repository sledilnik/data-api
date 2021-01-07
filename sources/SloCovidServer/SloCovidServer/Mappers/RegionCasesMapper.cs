using SloCovidServer.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class RegionCasesMapper : Mapper
    {
        public ImmutableArray<RegionCasesDay> GetDayFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var result = new List<RegionCasesDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var regions = ImmutableDictionary<string, RegionCasesDayData>.Empty;
                foreach (var pair in header)
                {
                    string[] parts = pair.Key.Split('.');
                    if (parts.Length >= 4 && (parts[0]?.Equals("region", System.StringComparison.Ordinal) ?? false))
                    {
                        string region = parts[1];
                        if (!regions.TryGetValue(region, out var dayData))
                        {
                            dayData = new RegionCasesDayData(null, null, null);
                        }
                        string key = string.Join('.', parts.Skip(2));
                        dayData = key switch
                        {
                            "cases.active" => dayData with { ActiveCases = GetInt(fields[pair.Value]) },
                            "cases.confirmed.todate" => dayData with { ConfirmedToDate = GetInt(fields[pair.Value]) },
                            "deceased.todate" => dayData with { DeceasedToDate = GetInt(fields[pair.Value]) },
                            _ => dayData,
                        };
                        regions = regions.SetItem(parts[1], dayData);
                    }
                }
                var date = GetDate(fields[dateIndex]);
                result.Add(new RegionCasesDay(
                    date.Year,
                    date.Month,
                    date.Day,
                    regions
                ));
            }
            return result.ToImmutableArray();
        }
    }
}
