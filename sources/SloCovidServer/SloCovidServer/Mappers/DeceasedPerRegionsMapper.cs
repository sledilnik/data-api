using SloCovidServer.Models;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SloCovidServer.Mappers
{
    public class DeceasedPerRegionsMapper: Mapper
    {
        public ImmutableArray<DeceasedPerRegionsDay> GetDeceasedPerRegionsDayFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var result = new List<DeceasedPerRegionsDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var regions = ImmutableDictionary<string, ImmutableDictionary<string, int?>>.Empty;
                foreach (var pair in header)
                {
                    string[] parts = pair.Key.Split('.');
                    if (parts.Length == 3 && (parts[0]?.Equals("region", System.StringComparison.Ordinal) ?? false))
                    {
                        if (!regions.TryGetValue(parts[1], out var region))
                        {
                            region = ImmutableDictionary<string, int?>.Empty;
                        }
                        region = region.Add(parts[2], GetInt(fields[pair.Value]));
                        regions = regions.SetItem(parts[1], region);
                    }
                }
                var date = GetDate(fields[dateIndex]);
                result.Add(new DeceasedPerRegionsDay(
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
