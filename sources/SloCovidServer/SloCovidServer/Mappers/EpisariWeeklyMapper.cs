using System.Collections.Generic;
using System.Collections.Immutable;
using SloCovidServer.Models;

namespace SloCovidServer.Mappers
{
    public class EpisariWeeklyMapper : Mapper
    {
        public ImmutableArray<EpisariWeek> GetEpisariWeeksFromRaw(string raw)
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
                result.Add(new EpisariWeek(week: fields[weekIndex], dateFrom, dateTo)
                {
                    Sari = new EpisariValueIn(new EpisariValue(GetInt(fields[sariInIndex]))),
                    Tested = new EpisariValueIn(new EpisariValue(GetInt(fields[testedInIndex]))),
                    Covid = new EpisariCovid
                    {
                        DiscoveredInHospital = GetInt(fields[covidDiscoveredInHospitalIndex]),
                        AcquiredInHospital = GetInt(fields[covidAcquiredInHospitalIndex]),
                        Deceased = new EpisariDeceased
                        {
                            Value = GetInt(fields[covidDeceasedIndex]),
                            PerAge = CollectAgeValues(header, fields, CovidDeceasedAgePrefix, GetIntAgeValue),
                        },
                        Out = new EpisariValueOut(new EpisariValue(GetInt(fields[covidOutIndex]))),
                        In = new EpisariCovidIn
                        {
                            Value = GetInt(fields[covidInIndex]),
                            Vaccination = new EpisariVaccinationStatus
                            {
                                Yes = GetInt(fields[covidInVaccIndex]),
                                No = GetInt(fields[covidInNotVaccIndex]),
                                Unknown = GetInt(fields[covidInVaccUnknownIndex]),
                                PerAge = CollectAgeValues(header, fields, CovidInVaccinationAge, GetIntAgeValue),
                            },
                            NotSari = GetInt(fields[covidInNotSariIndex]),
                            PerAge = CollectAgeValues(header, fields, CovidInAge, GetIntAgeValue),
                        },
                        Icu = new EpisariIcu
                        {
                            In = new EpisariIcuIn
                            {
                                Value = GetInt(fields[covidIcuInIndex]),
                                PerAge = CollectAgeValues(header, fields, CovidIcuInAge, GetIntAgeValue),
                            }
                        }
                    },
                });
            }
            return result.ToImmutableArray();
        }
    }
}
