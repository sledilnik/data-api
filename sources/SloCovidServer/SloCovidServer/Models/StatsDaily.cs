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
        public TestsAt14 TestsAt14 { get; }
        public PerTreatment StatePerTreatment { get; }
        public ImmutableDictionary<string, int?> StatePerRegion { get; }
        public ImmutableArray<PerAgeBucket> StatePerAgeToDate { get; }
        public ImmutableDictionary<string, int?> SourceToDate { get; }
        public ImmutableDictionary<string, int?> PerFacilityToDate { get; }
        public StatsDaily(int dayFromStart, int year, int month, int day, string phase, int? performedTestsToDate, int? performedTests, int? positiveTestsToDate,
            int? positiveTests, int? femaleToDate, int? maleToDate,
            TestsAt14 testsAt14, PerTreatment statePerTreatment, ImmutableDictionary<string, int?> statePerRegion,
            ImmutableArray<PerAgeBucket> statePerAgeToDate, ImmutableDictionary<string, int?> sourceToDate,
            ImmutableDictionary<string, int?> perFacilityToDate)
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
            TestsAt14 = testsAt14;
            StatePerTreatment = statePerTreatment;
            StatePerRegion = statePerRegion;
            StatePerAgeToDate = statePerAgeToDate;
            SourceToDate = sourceToDate;
            PerFacilityToDate = perFacilityToDate;
        }
    }

    /// <summary>
    /// Legacy reporting at 14:00. It was at beginning then NIJZ switched to 24:00.
    /// Obsolete.
    /// </summary>
    public class TestsAt14
    {
        public int? PerformedToDate { get; }
        public int? Performed { get; }
        public int? PositiveToDate { get; }
        public int? Positive { get; }
        public TestsAt14(int? performedToDate, int? performed, int? positiveToDate, int? positive)
        {
            PerformedToDate = performedToDate;
            Performed = performed;
            PositiveToDate = positiveToDate;
            Positive = positive;
        }
    }
}
