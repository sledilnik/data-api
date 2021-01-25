using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record StatsDaily : IModelDate
    {
        public int DayFromStart { get; init; }
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public string Phase { get; init; }
        // obsolete since 1.5.7
        public int? PerformedTestsToDate { get; init; }
        // obsolete since 1.5.7
        public int? PerformedTests { get; init; }
        // obsolete since 1.5.7
        public int? PositiveTestsToDate { get; init; }
        // obsolete since 1.5.7
        public int? PositiveTests { get; init; }
        public Tests Tests { get; init; }
        public int? FemaleToDate { get; init; }
        public int? MaleToDate { get; init; }
        public Cases Cases { get; init; }
        public PerTreatment StatePerTreatment { get; init; }
        public ImmutableDictionary<string, int?> StatePerRegion { get; init; }
        public ImmutableArray<PerAgeBucket> StatePerAgeToDate { get; init; }
        public ImmutableArray<PerAgeBucket> DeceasedPerAgeToDate { get; init; }
        public PerPersonType DeceasedPerType { get; init; }
        public int? DeceasedToDate { get; init; }
        public int? Deceased { get; init; }
        public Vaccination Vaccination { get; init; }
        public StatsDaily(int dayFromStart, int year, int month, int day, string phase, int? performedTestsToDate, int? performedTests, int? positiveTestsToDate,
            int? positiveTests, Tests tests,
            int? femaleToDate, int? maleToDate,
            Cases cases, PerTreatment statePerTreatment, ImmutableDictionary<string, int?> statePerRegion,
            ImmutableArray<PerAgeBucket> statePerAgeToDate, ImmutableArray<PerAgeBucket> deceasedPerAgeToDate,
            PerPersonType deceasedPerType, Vaccination vaccination, int? deceasedToDate, int? deceased)
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
            DeceasedPerType = deceasedPerType;
            Vaccination = vaccination;
            DeceasedToDate = deceasedToDate;
            Deceased = deceased;
        }
    }

    public record Tests
    {
        public TodayToDate Performed { get; init; }
        public TodayToDate Positive { get; init; }
        public RegularTests Regular { get; init; }
        public RegularTests NSApr20 { get; init; }
        public Tests(TodayToDate performed, TodayToDate positive, RegularTests regular, RegularTests nSApr20)
        {
            Performed = performed;
            Positive = positive;
            Regular = regular;
            NSApr20 = nSApr20;
        }
    }

    public record RegularTests
    {
        public TodayToDate Performed { get; init; }
        public TodayToDate Positive { get; init; }
        public RegularTests(TodayToDate performed, TodayToDate positive)
        {
            Performed = performed;
            Positive = positive;
        }
    }
    public record Cases
    {
        public int? ConfirmedToday { get; init; }
        public int? ConfirmedToDate { get; init; }
        public int? RecoveredToDate { get; init; }
        public int? ClosedToDate { get; init; }
        // obsolete since 1.5.8
        public int? ActiveToDate { get; init; }
        public int? Active { get; init; }
        public HealthSystemSCases HS { get; init; }
        public RetirementHomeCases RH { get; init; }
        public UnclassifiedCases Unclassified { get; init; }
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
    public record UnclassifiedCases
    {
        public int? ConfirmedToDate { get; init; }
        public UnclassifiedCases(int? confirmedToDate)
        {
            ConfirmedToDate = confirmedToDate;
        }
    }

    /// <summary>
    /// Health system cases
    /// </summary>
    public record HealthSystemSCases
    {
        public int? EmployeeConfirmedToDate { get; init; }
        public HealthSystemSCases(int? employeeConfirmedToDate)
        {
            EmployeeConfirmedToDate = employeeConfirmedToDate;
        }
    }
    /// <summary>
    /// Retirement home cases
    /// </summary>
    public record RetirementHomeCases
    {
        public int? EmployeeConfirmedToDate { get; init; }
        public int? OccupantConfirmedToDate { get; init; }
        public RetirementHomeCases(int? employeeConfirmedToDate, int? occupantConfirmedToDate)
        {
            EmployeeConfirmedToDate = employeeConfirmedToDate;
            OccupantConfirmedToDate = occupantConfirmedToDate;
        }
    }
    public record Vaccination
    {                
        public TodayToDate Administered { get; init; }
        public TodayToDate Administered2nd { get; init; }
        public TodayToDate Used { get; init; }
        public TodayToDate Delivered { get; init; }
        public Vaccination(TodayToDate administered, TodayToDate administered2nd, TodayToDate used, TodayToDate delivered)
        {
            Administered = administered;
            Administered2nd = administered2nd;
            Used = used;
            Delivered = delivered;
        }
    }
}
