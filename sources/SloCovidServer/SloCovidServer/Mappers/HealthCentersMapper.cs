using NJsonSchema;
using SloCovidServer.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class HealthCentersMapper : Mapper
    {
        public ImmutableArray<HealthCentersDay> GetHealthCentersDayFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var result = new List<HealthCentersDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var regions = ImmutableDictionary<string, ImmutableDictionary<string, HealthCentersDayItem>>.Empty;
                var all = HealthCentersDayItem.Empty;
                foreach (var pair in header)
                {
                    string[] parts = pair.Key.Split('.');
                    if (parts[0]?.Equals("hc", System.StringComparison.Ordinal) ?? false)
                    {
                        string key;
                        int? value = GetInt(fields[pair.Value]);
                        switch (parts.Length)
                        {
                            case 3:
                                key = string.Join('.', parts.Skip(1));
                                all = UpdateItem(all, key, value);
                                break;
                            case 5:
                                if (!regions.TryGetValue(parts[1], out var region))
                                {
                                    region = ImmutableDictionary<string, HealthCentersDayItem>.Empty;
                                }
                                if (!region.TryGetValue(parts[2], out var municipality))
                                {
                                    municipality = HealthCentersDayItem.Empty;
                                }
                                key = string.Join('.', parts.Skip(3));
                                municipality = UpdateItem(municipality, key, value);
                                region = region.SetItem(parts[2], municipality);
                                regions = regions.SetItem(parts[1], region);
                                break;
                        }
                    }
                }
                var date = GetDate(fields[dateIndex]);
                result.Add(new HealthCentersDay(
                    date.Year,
                    date.Month,
                    date.Day,
                    all,
                    regions
                ));
            }
            return result.ToImmutableArray();
        }

        HealthCentersDayItem UpdateItem(HealthCentersDayItem source, string key, int? value) =>
            key switch
            {
                "examinations.medical_emergency" => source = source with { Examinations = source.Examinations with { MedicalEmergency = value } },
                "examinations.suspected_covid" => source with { Examinations = source.Examinations with { SuspectedCovid = value } },
                "phone_triage.suspected_covid" => source with { PhoneTriage = source.PhoneTriage with { SuspectedCovid = value } },
                "tests.performed" => source with { Tests = source.Tests with { Performed = value } },
                "tests.positive" => source with { Tests = source.Tests with { Positive = value } },
                "sent_to.hospital" => source with { SentTo = source.SentTo with { Hospital = value } },
                "sent_to.self_isolation" => source with { SentTo = source.SentTo with { SelfIsolation = value } },
                _ => source,
            };
    }
}
