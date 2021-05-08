using SloCovidServer.Models;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class VaccinationByMunicipalityMapper: Mapper
    {
        public ImmutableArray<VaccinationByMunicipalityDay> GetFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var query = from l in IterateLines(lines)
                        let fields = ParseLine(l)
                        let date = GetDate(fields[dateIndex])
                        let data = ExtractData(header, fields)
                        select new VaccinationByMunicipalityDay
                        (
                            date.Year,
                            date.Month,
                            date.Day,
                            Regions: data
                        );
            return query.ToImmutableArray();
        }

        public ImmutableDictionary<string, ImmutableDictionary<string, VaccinationByMunicipalityToDate>> ExtractData(ImmutableDictionary<string, int> header, 
            ImmutableArray<string> fields)
        {
            ImmutableDictionary<string, ImmutableDictionary<string, VaccinationByMunicipalityToDate>> result 
                = ImmutableDictionary<string, ImmutableDictionary<string, VaccinationByMunicipalityToDate>>.Empty;
            foreach (var pair in header)
            {
                var parts = pair.Key.Split('.');
                if (parts.Length == 6)
                {
                    int? value = GetInt(fields[pair.Value]);
                    string regionKey = parts[2];
                    if (!result.TryGetValue(regionKey, out var region))
                    {
                        region = ImmutableDictionary<string, VaccinationByMunicipalityToDate>.Empty;
                    }
                    string municipalityKey = parts[3];
                    if (!region.TryGetValue(municipalityKey, out var municipality))
                    {
                        municipality = new VaccinationByMunicipalityToDate(null, null);
                    }
                    switch (parts[4])
                    {
                        case "1st":
                            municipality = municipality with { First = value };
                            break;
                        case "2nd":
                            municipality = municipality with { Second = value };
                            break;
                    }
                    region = region.SetItem(municipalityKey, municipality);
                    result = result.SetItem(regionKey, region);
                }
            }
            return result;
        }
    }
}
