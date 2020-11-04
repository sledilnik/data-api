using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public class StatsWeeklyDay: IModelDate
    {
        public string Week { get; }
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public Date To { get; }
        public int? Confirmed { get; }
        public int? Investigated { get; }
        public int? Healthcare { get; }
        public StatsWeeklySentTo SentTo { get; }
        public ImmutableDictionary<string, int?> Source { get; }
        public ImmutableDictionary<string, int?> From { get; }
        public StatsWeeklyDay(string week, int year, int month, int day, Date to, int? confirmed, int? investigated, int? healthcare, StatsWeeklySentTo sentTo, ImmutableDictionary<string, int?> source, 
            ImmutableDictionary<string, int?> from)
        {
            Week = week;
            Year = year;
            Month = month;
            Day = day;
            To = to;
            Confirmed = confirmed;
            Investigated = investigated;
            Healthcare = healthcare;
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
