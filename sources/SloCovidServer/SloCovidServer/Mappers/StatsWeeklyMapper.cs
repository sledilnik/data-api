using SloCovidServer.Models;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SloCovidServer.Mappers
{
    public class StatsWeeklyMapper : Mapper
    {
        public ImmutableArray<StatsWeeklyDay> GetStatsWeeklyDayFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            int weekIndex = header["week"];
            int dateToIndex = header["date.to"];
            int weekHospitalizedVaccinatedIndex = header["week.hospitalized.vaccinated"];
            int weekHospitalizedOtherIndex = header["week.hospitalized.other"];
            int weekIcuVaccinatedIndex = header["week.icu.vaccinated"];
            int weekIcuVaccinatedPartiallyIndex = header["week.icu.vaccinatedpartially"];
            int weekIcuRecoveredIndex = header["week.icu.recovered"];
            int weekIcuOtherIndex = header["week.icu.other"];
            int weekConfirmedIndex = header["week.confirmed"];
            int weekInvestigatedIndex = header["week.investigated"];
            int weekHealthcareIndex = header["week.healthcare"];
            int weekHealthcareMaleIndex = header["week.healthcare.male"];
            int weekHealthcareFemaleIndex = header["week.healthcare.female"];
            int weekRhOccupantIndex = header["week.rhoccupant"];
            int weekSentToQuarantineIndex = header["week.sent_to.quarantine"];
            var result = new List<StatsWeeklyDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var date = GetDate(fields[dateIndex]);
                var dateTo = GetDate(fields[dateToIndex]);
                var sentTo = new StatsWeeklySentTo(GetInt(fields[weekSentToQuarantineIndex]));
                var source = ImmutableDictionary<string, int?>.Empty;
                var from = ImmutableDictionary<string, int?>.Empty;
                var loc = ImmutableDictionary<string, int?>.Empty;
                foreach (var pair in header)
                {
                    string[] parts = pair.Key.Split('.');
                    switch (parts.Length)
                    {
                        case 3:
                            int? value;
                            switch (parts[1])
                            {
                                case "src":
                                    value = GetInt(fields[pair.Value]);
                                    source = source.Add(parts[2], value);
                                    break;
                                case "from":
                                    value = GetInt(fields[pair.Value]);
                                    from = from.Add(parts[2], value);
                                    break;
                                case "loc":
                                    value = GetInt(fields[pair.Value]);
                                    loc = loc.Add(parts[2], value);
                                    break;
                            }
                            break;
                    }
                }
                result.Add(new StatsWeeklyDay(
                week: fields[weekIndex],
                date.Year,
                date.Month,
                date.Day,
                dateTo,
                GetInt(fields[weekHospitalizedVaccinatedIndex]),
                GetInt(fields[weekHospitalizedOtherIndex]),
                GetInt(fields[weekIcuVaccinatedIndex]),
                GetInt(fields[weekIcuVaccinatedPartiallyIndex]),
                GetInt(fields[weekIcuRecoveredIndex]),
                GetInt(fields[weekIcuOtherIndex]),
                GetInt(fields[weekConfirmedIndex]),
                GetInt(fields[weekInvestigatedIndex]),
                GetInt(fields[weekHealthcareIndex]),
                GetInt(fields[weekHealthcareMaleIndex]),
                GetInt(fields[weekHealthcareFemaleIndex]),
                GetInt(fields[weekRhOccupantIndex]),
                sentTo: sentTo,
                source,
                from,
                loc
            ));
            }
            return result.ToImmutableArray();
        }
    }
}
