using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public class StatsDaily
    {
        public int DayFromStart { get; }
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public string Phase { get; }
        public int? PerformedTestsToDate { get; }
        public int? PerformedTests { get; }
        public int? PositiveTestsToDate { get; }
        public int? PositiveTests { get; }
        public int? FemaleToDate { get; }
        public int? MaleToDate { get; }
        public Cases Cases { get; }
        public PerTreatment StatePerTreatment { get; }
        public ImmutableDictionary<string, int?> StatePerRegion { get; }
        public ImmutableArray<PerAgeBucket> StatePerAgeToDate { get; }
        public StatsDaily(int dayFromStart, int year, int month, int day, string phase, int? performedTestsToDate, int? performedTests, int? positiveTestsToDate,
            int? positiveTests, int? femaleToDate, int? maleToDate,
            Cases cases, PerTreatment statePerTreatment, ImmutableDictionary<string, int?> statePerRegion,
            ImmutableArray<PerAgeBucket> statePerAgeToDate)
        {
            DayFromStart = dayFromStart;
            Year = year;
            Month = month;
            Day = day;
            Phase = phase;
            PerformedTestsToDate = performedTestsToDate;
            PerformedTests = performedTests;
            PositiveTestsToDate = positiveTestsToDate;
            PositiveTests = positiveTests;
            FemaleToDate = femaleToDate;
            MaleToDate = maleToDate;
            Cases = cases;
            StatePerTreatment = statePerTreatment;
            StatePerRegion = statePerRegion;
            StatePerAgeToDate = statePerAgeToDate;
        }
    }

    public class Cases
    {
        public int? ConfirmedToday { get; }
        public int? ConfirmedToDate { get; }
        public int? ClosedToDate { get; }
        public int? ActiveToDate { get; }
        public Cases(int? confirmedToday, int? confirmedToDate, int? closedToDate, int? activeToDate)
        {
            ConfirmedToday = confirmedToday;
            ConfirmedToDate = confirmedToDate;
            ClosedToDate = closedToDate;
            ActiveToDate = activeToDate;
        }
    }
}
