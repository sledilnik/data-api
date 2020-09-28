using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public class StatsWeeklyDay: IModelDate
    {
        public int Week { get; }
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public Date To { get; }
        public int? Confirmed { get; }
        public StatsWeeklySentTo SentTo { get; }
        public ImmutableDictionary<string, int?> Source { get; }
        public ImmutableDictionary<string, int?> From { get; }
        public StatsWeeklyDay(int week, int year, int month, int day, Date to, int? confirmed, StatsWeeklySentTo sentTo, ImmutableDictionary<string, int?> source, 
            ImmutableDictionary<string, int?> from)
        {
            Week = week;
            Year = year;
            Month = month;
            Day = day;
            To = to;
            Confirmed = confirmed;
            SentTo = sentTo;
            Source = source;
            From = from;
        }
    }

    public class StatsWeeklySentTo
    {
        public int? Quarantine { get; }
        public StatsWeeklySentTo(int? quarantine)
        {
            Quarantine = quarantine;
        }
    }
}
