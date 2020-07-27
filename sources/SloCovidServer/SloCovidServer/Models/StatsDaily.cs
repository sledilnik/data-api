using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public class StatsDaily: IModelDate
    {
        public int DayFromStart { get; }
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public string Phase { get; }
        // obsolete since 1.5.7
        public int? PerformedTestsToDate { get; }
        // obsolete since 1.5.7
        public int? PerformedTests { get; }
        // obsolete since 1.5.7
        public int? PositiveTestsToDate { get; }
        // obsolete since 1.5.7
        public int? PositiveTests { get; }
        public Tests Tests { get; }
        public int? FemaleToDate { get; }
        public int? MaleToDate { get; }
        public Cases Cases { get; }
        public PerTreatment StatePerTreatment { get; }
        public ImmutableDictionary<string, int?> StatePerRegion { get; }
        public ImmutableArray<PerAgeBucket> StatePerAgeToDate { get; }
        public ImmutableArray<PerAgeBucket> DeceasedPerAgeToDate { get; }
        public StatsDaily(int dayFromStart, int year, int month, int day, string phase, int? performedTestsToDate, int? performedTests, int? positiveTestsToDate,
            int? positiveTests, Tests tests,
            int? femaleToDate, int? maleToDate,
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
            Tests = tests;
            FemaleToDate = femaleToDate;
            MaleToDate = maleToDate;
            Cases = cases;
            StatePerTreatment = statePerTreatment;
            StatePerRegion = statePerRegion;
            StatePerAgeToDate = statePerAgeToDate;
            DeceasedPerAgeToDate = deceasedPerAgeToDate;
        }
    }

    public class Tests
    {
        public CommonTests Performed { get; }
        public CommonTests Positive { get; }
        public RegularTests Regular { get; }
        public RegularTests NSApr20 { get; }
        public Tests(CommonTests performed, CommonTests positive, RegularTests regular, RegularTests nSApr20)
        {
            Performed = performed;
            Positive = positive;
            Regular = regular;
            NSApr20 = nSApr20;
        }
    }

    public class RegularTests
    {
        public CommonTests Performed { get; }
        public CommonTests Positive { get; }
        public RegularTests(CommonTests performed, CommonTests positive)
        {
            Performed = performed;
            Positive = positive;
        }
    }

    public class CommonTests
    {
        public int? ToDate { get; }
        public int? Today { get; }
        public CommonTests(int? toDate, int? today)
        {
            ToDate = toDate;
            Today = today;
        }
    }

    public class Cases
    {
        public int? ConfirmedToday { get; }
        public int? ConfirmedToDate { get; }
        public int? RecoveredToDate { get; }
        public int? ClosedToDate { get; }
        // obsolete since 1.5.8
        public int? ActiveToDate { get; }
        public int? Active { get; }
        public HealthSystemSCases HS { get; }
        public RetirementHomeCases RH { get; }
        public UnclassifiedCases Unclassified { get; }
        public Cases(int? confirmedToday, int? confirmedToDate, int? recoveredToDate, int? closedToDate, int? active, HealthSystemSCases hs, RetirementHomeCases rh,
            UnclassifiedCases unclassified)
        {
            ConfirmedToday = confirmedToday;
            ConfirmedToDate = confirmedToDate;
            RecoveredToDate = recoveredToDate;
            ClosedToDate = closedToDate;
            Active = active;
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
