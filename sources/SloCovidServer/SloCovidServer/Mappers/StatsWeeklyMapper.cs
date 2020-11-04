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
            int weekConfirmedIndex = header["week.confirmed"];
            int weekInvestigatedIndex = header["week.investigated"];
            int weekHealthcareIndex = header["week.healthcare"];
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
                foreach (var pair in header)
                {
                    string[] parts = pair.Key.Split('.');
                    switch (parts.Length)
                    {
                        case 3:
                            if (string.Equals(parts[1], "src", System.StringComparison.Ordinal))
                            {
                                int? value = GetInt(fields[pair.Value]);
                                source = source.Add(parts[2], value);
                            }
                            else if (string.Equals(parts[1], "from", System.StringComparison.Ordinal))
                            {
                                int? value = GetInt(fields[pair.Value]);
                                from = from.Add(parts[2], value);
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
                GetInt(fields[weekConfirmedIndex]),
                GetInt(fields[weekInvestigatedIndex]),
                GetInt(fields[weekHealthcareIndex]),
                sentTo: sentTo,
                source,
                from
            ));
            }
            return result.ToImmutableArray();
        }
    }
}
