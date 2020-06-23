using SloCovidServer.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class MunicipalitiesMapper : Mapper
    {
        public ImmutableArray<MunicipalityDay> GetMunicipalityDayFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var result = new List<MunicipalityDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var regions = ImmutableDictionary<string, ImmutableDictionary<string, MunicipalityDayData>>.Empty;
                foreach (var pair in header)
                {
                    string[] parts = pair.Key.Split('.');
                    if (parts.Length >= 5 && (parts[0]?.Equals("region", System.StringComparison.Ordinal) ?? false))
                    {
                        if (!regions.TryGetValue(parts[1], out var region))
                        {
                            region = ImmutableDictionary<string, MunicipalityDayData>.Empty;
                        }
                        if (!region.TryGetValue(parts[2], out var municipality))
                        {
                            municipality = new MunicipalityDayData(0, 0, 0);
                        }
                        string key = string.Join('.', parts.Skip(3));
                        switch (key)
                        {
                            case "cases.active":
                                municipality = new MunicipalityDayData(GetInt(fields[pair.Value]), municipality.ConfirmedToDate, municipality.DeceasedToDate);
                                break;
                            case "cases.confirmed.todate":
                                municipality = new MunicipalityDayData(municipality.ActiveCases, GetInt(fields[pair.Value]), municipality.DeceasedToDate);
                                break;
                            case "deceased.todate":
                                municipality = new MunicipalityDayData(municipality.ActiveCases, municipality.ConfirmedToDate, GetInt(fields[pair.Value]));
                                break;
                        }
                        region = region.SetItem(parts[2], municipality);
                        regions = regions.SetItem(parts[1], region);
                    }
                }
                var date = GetDate(fields[dateIndex]);
                result.Add(new MunicipalityDay(
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
