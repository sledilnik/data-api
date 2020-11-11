using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record StatsWeeklyDay : IModelDate
    {
        public string Week { get; init; }
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public Date To { get; init; }
        public int? Confirmed { get; init; }
        public int? Investigated { get; init; }
        public int? Healthcare { get; init; }
        public StatsWeeklySentTo SentTo { get; init; }
        public ImmutableDictionary<string, int?> Source { get; init; }
        public ImmutableDictionary<string, int?> From { get; init; }
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

    public record StatsWeeklySentTo
    {
        public int? Quarantine { get; init; }
        public StatsWeeklySentTo(int? quarantine)
        {
            Quarantine = quarantine;
        }
    }
}
