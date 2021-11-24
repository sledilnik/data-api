using SloCovidServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SloCovidServer.Mappers;

namespace SloCovidServer.Mappers
{
    public class VaccinationMapper : Mapper
    {
        static readonly ImmutableArray<AgeBucketMeta> ageBuckets;
        static readonly int[] ageBucketRanges = new[] { 17, 24, 29, 34, 39, 44, 49, 54, 59, 64, 69, 74, 79, 84, 89 };

        static VaccinationMapper()
        {
            ageBuckets = ImmutableArray<AgeBucketMeta>.Empty;
            int start = 0;
            foreach (int r in ageBucketRanges)
            {
                ageBuckets = ageBuckets.Add(new AgeBucketMeta(start, r));
                start = r + 1;
            }
            ageBuckets = ageBuckets.Add(new AgeBucketMeta(start, null));
        }
        public ImmutableArray<VaccinationDay> GetVaccinationsFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            int administeredIndex = header["vaccination.administered"];
            int administeredToDateIndex = header["vaccination.administered.todate"];
            int administered2ndIndex = header["vaccination.administered2nd"];
            int administered2ndToDateIndex = header["vaccination.administered2nd.todate"];
            int administered3rdIndex = header["vaccination.administered3rd"];
            int administered3rdToDateIndex = header["vaccination.administered3rd.todate"];
            int usedToDateIndex = header["vaccination.used.todate"];
            var usedByManufacturersIndex = header
                .Select(h => new { Parts = h.Key.Split('.'), Index = h.Value })
                .Where(h => h.Parts.Length == 4
                    && string.Equals(h.Parts[0], "vaccination", StringComparison.Ordinal)
                    && string.Equals(h.Parts[2], "used", StringComparison.Ordinal)
                    && string.Equals(h.Parts[3], "todate", StringComparison.Ordinal))
                .Select(h => new { Manufacturer = h.Parts[1], Index = h.Index })
                .ToImmutableArray();
            int deliveredToDateIndex = header["vaccination.delivered.todate"];
            var deliveredByManufacturersIndex = header
                .Select(h => new { Parts = h.Key.Split('.'), Index = h.Value })
                .Where(h => h.Parts.Length == 4
                    && string.Equals(h.Parts[0], "vaccination", StringComparison.Ordinal)
                    && string.Equals(h.Parts[2], "delivered", StringComparison.Ordinal)
                    && string.Equals(h.Parts[3], "todate", StringComparison.Ordinal))
                .Select(h => new { Manufacturer = h.Parts[1], Index = h.Index })
                .ToImmutableArray();
            var result = new List<VaccinationDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var date = GetDate(fields[dateIndex]);
                var usedByManufacturer = usedByManufacturersIndex
                    .Select(i => new { Manufacturer = i.Manufacturer, Value = GetInt(fields[i.Index]) })
                    .Where(v => v.Value.HasValue)
                    .ToImmutableDictionary(v => v.Manufacturer, v => v.Value.Value);
                var deliveredByManufacturer = deliveredByManufacturersIndex
                    .Select(i => new { Manufacturer = i.Manufacturer, Value = GetInt(fields[i.Index]) })
                    .Where(v => v.Value.HasValue)
                    .ToImmutableDictionary(v => v.Manufacturer, v => v.Value.Value);
                var perAgeVaccinated = ImmutableArray<PerAgeBucket>.Empty;
                foreach (var bucket in ageBuckets)
                {
                    var perAge = new PerAgeBucket(
                        bucket.AgeFrom,
                        bucket.AgeTo,
                        null, null, null,
                        GetInt($"vaccination.age.{bucket.Key}.1st.todate", header, fields),
                        GetInt($"vaccination.age.{bucket.Key}.2nd.todate", header, fields),
                        GetInt($"vaccination.age.{bucket.Key}.3rd.todate", header, fields)
                    );
                    perAgeVaccinated = perAgeVaccinated.Add(perAge);
                }
                var item = new VaccinationDay(date.Year, date.Month, date.Day,
                    Administered: new VaccinationData(GetInt(fields[administeredIndex]), GetInt(fields[administeredToDateIndex])),
                    Administered2nd: new VaccinationData(GetInt(fields[administered2ndIndex]), GetInt(fields[administered2ndToDateIndex])),
                    Administered3rd: new VaccinationData(GetInt(fields[administered3rdIndex]), GetInt(fields[administered3rdToDateIndex])),
                    UsedToDate: GetInt(fields[usedToDateIndex]),
                    UsedByManufacturer: usedByManufacturer,
                    DeliveredToDate: GetInt(fields[deliveredToDateIndex]),
                    DeliveredByManufacturer: deliveredByManufacturer,
                    perAgeVaccinated
                );
                result.Add(item);
            }
            return result.ToImmutableArray();
        }
    }
}
