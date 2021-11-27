using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SloCovidServer.Models;

namespace SloCovidServer.Mappers
{
    public class EpisariWeeklyMapper : Mapper
    {
        internal EpisariPerAgeBucket ConvertToBucket(string age, EpisariPerAge data)
        {
            var (from, to) = ExtractAges(age);
            return new EpisariPerAgeBucket(from, to, data.CovidIn, data.VaccinatedIn, data.IcuIn, data.Deceased);
        }

        /// <summary>
        /// Parses formats 'From-To', 'From+' and 'mean'
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        internal (int? From, int? To) ExtractAges(string age)
        {
            if (string.Equals(age, "mean", System.StringComparison.Ordinal))
            {
                return (default, default);
            }
            int index = age.IndexOf('+');
            if (index < 0)
            {
                index = age.IndexOf('-');
            }
            int? from = null;
            int? to = null;
            if (int.TryParse(age[0..index], out int temp))
            {
                from = temp;
            }
            if (int.TryParse(age[(index + 1)..], out temp))
            {
                to = temp;
            }
            return (from, to);
        }
        public async Task<ImmutableArray<EpisariWeek>> GetEpisariWeeksFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int weekIndex = header["episari.week"];
            int dateFromIndex = header["episari.date.from"];
            int dateToIndex = header["episari.date.to"];
            int sourceIndex = header["episari.source"];
            int missingIndex = header["episari.missing"];
            int sariInIndex = header["episari.sari.in"];
            int testedInIndex = header["episari.tested.in"];
            int covidDiscoveredInHospitalIndex = header["episari.covid.discoveredinhospital"];
            int covidAcquiredInHospitalIndex = header["episari.covid.acquiredinhospital"];
            int covidDeceasedIndex = header["episari.covid.deceased"];
            const string CovidDeceasedAgePrefix = "episari.covid.deceased.age";
            int covidOutIndex = header["episari.covid.out"];
            int covidInIndex = header["episari.covid.in"];
            int covidInVaccIndex = header["episari.covid.in.vacc"];
            int covidInNotVaccIndex = header["episari.covid.in.notvacc"];
            int covidInVaccUnknownIndex = header["episari.covid.in.vaccunknown"];
            const string CovidInVaccinationAge = "episari.covid.in.vacc.age";
            int covidInNotSariIndex = header["episari.covid.in.notsari"];
            const string CovidInAge = "episari.covid.in.age";
            int covidIcuInIndex = header["episari.covid.icu.in"];
            const string  CovidIcuInAge = "episari.covid.icu.in.age";
            var result = new List<EpisariWeek>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var dateFrom = GetDate(fields[dateFromIndex]);
                var dateTo = GetDate(fields[dateToIndex]);
                var perAge = new ConcurrentDictionary<string, EpisariPerAge>();
                var covidInPerAgeTask = Task.Run(() =>
                {
                    var data = CollectAgeValues(header, fields, CovidInAge, GetIntAgeValue);
                    foreach (var d in data)
                    {
                        var item = perAge.AddOrUpdate(d.Key,
                            new EpisariPerAge(d.Value, default, default, default),
                            (k, v) => v with { CovidIn = d.Value });
                    }
                });
                var vaccinatedInPerAgeTask = Task.Run(() =>
                {
                    var data = CollectAgeValues(header, fields, CovidInVaccinationAge, GetIntAgeValue);
                    foreach (var d in data)
                    {
                        var item = perAge.AddOrUpdate(d.Key,
                            new EpisariPerAge(default, d.Value, default, default),
                            (k, v) => v with { VaccinatedIn = d.Value });
                    }
                });
                var icuInPerAgeTask = Task.Run(() =>
                {
                    var data = CollectAgeValues(header, fields, CovidIcuInAge, GetIntAgeValue);
                    foreach (var d in data)
                    {
                        var item = perAge.AddOrUpdate(d.Key,
                            new EpisariPerAge(default, default, d.Value, default),
                            (k, v) => v with { IcuIn = d.Value });
                    }
                });
                var deceasedPerAgeTask = Task.Run(() =>
                {
                    var data = CollectAgeValues(header, fields, CovidDeceasedAgePrefix, GetIntAgeValue);
                    foreach (var d in data)
                    {
                        var item = perAge.AddOrUpdate(d.Key,
                            new EpisariPerAge(default, default, default, d.Value),
                            (k, v) => v with { Deceased = d.Value });
                    }
                });
                await Task.WhenAll(covidInPerAgeTask, vaccinatedInPerAgeTask, deceasedPerAgeTask, icuInPerAgeTask);
                result.Add(new EpisariWeek(week: fields[weekIndex], dateFrom, dateTo)
                {
                    Source = fields[sourceIndex],
                    Missing = fields[missingIndex],
                    SariIn = GetInt(fields[sariInIndex]),
                    TestedIn = GetInt(fields[testedInIndex]),
                    CovidIn = GetInt(fields[covidInIndex]),
                    CovidOut = GetInt(fields[covidOutIndex]),
                    CovidInNotSari = GetInt(fields[covidInNotSariIndex]),
                    CovidInVaccinated = GetInt(fields[covidInVaccIndex]),
                    CovidInVaccinatedUnknown = GetInt(fields[covidInVaccUnknownIndex]),
                    CovidInNotVaccinated = GetInt(fields[covidInNotVaccIndex]),
                    CovidDiscoveredInHospital = GetInt(fields[covidDiscoveredInHospitalIndex]),
                    CovidAcquiredInHospital = GetInt(fields[covidAcquiredInHospitalIndex]),
                    CovidDeceased = GetInt(fields[covidDeceasedIndex]),
                    CovidIcuIn = GetInt(fields[covidIcuInIndex]),
                    PerAge = perAge.Select(d => ConvertToBucket(d.Key, d.Value)).ToImmutableArray(),
                });
            }
            return result.ToImmutableArray();
        }
    }
}
