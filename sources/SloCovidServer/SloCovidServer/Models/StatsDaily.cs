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
        public PerTreatment StatePerTreatment { get; }
        public ImmutableDictionary<string, int?> StatePerRegion { get; }
        public ImmutableArray<PerAgeBucket> StatePerAgeToDate { get; }
        public ImmutableDictionary<string, int?> SourceToDate { get; }
        public ImmutableDictionary<string, int?> PerFacilityToDate { get; }
        public StatsDaily(int dayFromStart, int year, int month, int day, string phase, int? performedTestsToDate, int? performedTests, int? positiveTestsToDate,
            int? positiveTests, int? femaleToDate, int? maleToDate,
            PerTreatment statePerTreatment, ImmutableDictionary<string, int?> statePerRegion,
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
            StatePerTreatment = statePerTreatment;
            StatePerRegion = statePerRegion;
            StatePerAgeToDate = statePerAgeToDate;
            SourceToDate = sourceToDate;
            PerFacilityToDate = perFacilityToDate;
        }
    }
}
