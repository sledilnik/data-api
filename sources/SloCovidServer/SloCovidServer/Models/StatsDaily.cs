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
        public ImmutableArray<PerAgeBucket> DeceasedPerAgeToDate { get; }
        public StatsDaily(int dayFromStart, int year, int month, int day, string phase, int? performedTestsToDate, int? performedTests, int? positiveTestsToDate,
            int? positiveTests, int? femaleToDate, int? maleToDate,
            Cases cases, PerTreatment statePerTreatment, ImmutableDictionary<string, int?> statePerRegion,
            ImmutableArray<PerAgeBucket> statePerAgeToDate, ImmutableArray<PerAgeBucket> deceasedPerAgeToDate)
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
            DeceasedPerAgeToDate = deceasedPerAgeToDate;
        }
    }

    public class Cases
    {
        public int? ConfirmedToday { get; }
        public int? ConfirmedToDate { get; }
        public int? ClosedToDate { get; }
        public int? ActiveToDate { get; }
        public HealthSystemSCases HS { get; }
        public RetirementHomeCases RH { get; }
        public UnclassifiedCases Unclassified { get; }
        public Cases(int? confirmedToday, int? confirmedToDate, int? closedToDate, int? activeToDate, HealthSystemSCases hs, RetirementHomeCases rh,
            UnclassifiedCases unclassified)
        {
            ConfirmedToday = confirmedToday;
            ConfirmedToDate = confirmedToDate;
            ClosedToDate = closedToDate;
            ActiveToDate = activeToDate;
            HS = hs;
            RH = rh;
            Unclassified = unclassified;
        }
    }

    public class UnclassifiedCases
    {
        public int? ConfirmedToDate { get; }
        public UnclassifiedCases(int? confirmedToDate)
        {
            ConfirmedToDate = confirmedToDate;
        }
    }

    /// <summary>
    /// Health system cases
    /// </summary>
    public class HealthSystemSCases
    {
        public int? EmployeeConfirmedToDate { get; }
        public HealthSystemSCases(int? employeeConfirmedToDate)
        {
            EmployeeConfirmedToDate = employeeConfirmedToDate;
        }
    }
    /// <summary>
    /// Retirement home cases
    /// </summary>
    public class RetirementHomeCases
    {
        public int? EmployeeConfirmedToDate { get; }
        public int? OccupantConfirmedToDate { get; }
        public RetirementHomeCases(int? employeeConfirmedToDate, int? occupantConfirmedToDate)
        {
            EmployeeConfirmedToDate = employeeConfirmedToDate;
            OccupantConfirmedToDate = occupantConfirmedToDate;
        }
    }
}
