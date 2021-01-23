using SloCovidServer.Models;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SloCovidServer.Mappers
{
    public class SewageMapper : Mapper
    {
        public ImmutableArray<SewageDay> GetFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var result = new List<SewageDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var plants = ImmutableDictionary<string, ImmutableDictionary<string, float?>>.Empty;
                foreach (var pair in header)
                {
                    string[] parts = pair.Key.Split('.');
                    if (parts.Length == 3)
                    {
                        float? value = GetFloat(fields[pair.Value]);
                        string plantName = parts[1];
                        if (value.HasValue)
                        {
                            if (!plants.TryGetValue(plantName, out var plant))
                            {
                                plant = ImmutableDictionary<string, float?>.Empty;
                            }
                            plant = plant.Add(parts[2], value);
                            plants = plants.SetItem(plantName, plant);
                        }
                    }
                }
                if (plants.Count > 0)
                {
                    var date = GetDate(fields[dateIndex]);
                    result.Add(new SewageDay(
                        date.Year,
                        date.Month,
                        date.Day,
                        plants
                    ));
                }
            }
            return result.ToImmutableArray();
        }
    }
}
