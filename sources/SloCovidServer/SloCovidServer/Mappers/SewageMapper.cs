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
                var cities = ImmutableDictionary<string, SewageCityDay>.Empty;
                foreach (var pair in header)
                {
                    string[] parts = pair.Key.Split('.');
                    if (parts.Length == 3)
                    {
                        string cityName = parts[1];
                        if (!cities.TryGetValue(cityName, out var city))
                        {
                            city = new SewageCityDay(ImmutableDictionary<string, float?>.Empty);
                        }
                        city = city with { Measurements = city.Measurements.Add(parts[2], GetFloat(fields[pair.Value])) };
                        cities = cities.SetItem(cityName, city);
                    }
                }
                var date = GetDate(fields[dateIndex]);
                result.Add(new SewageDay(
                    date.Year,
                    date.Month,
                    date.Day,
                    cities
                ));
            }
            return result.ToImmutableArray();
        }
    }
}
