namespace SloCovidServer.Models
{
    /// <summary>
    /// Total summary
    /// </summary>
    public record Summary(
        CasesToDateSummary CasesToDateSummary,
        CasesActive CasesActive,
        CasesAvg7Days CasesAvg7Days,
        HospitalizedCurrent HospitalizedCurrent,
        ICUCurrent ICUCurrent,
        DeceasedToDate DeceasedToDate);
    /// <summary>
    /// Base class for summary items
    /// </summary>
    public abstract record SummaryBase(float? DiffPercentage, int Year, int Month, int Date);
    public record CasesToDateSummary(int? Number, int? In, float? DiffPercentage, int Year, int Month, int Day)
        : SummaryBase(DiffPercentage, Year, Month, Day);
    public record CasesActive(int? Total, int? In, int? Out, float? DiffPercentage, int Year, int Month, int Day)
        : SummaryBase(DiffPercentage, Year, Month, Day);
    public record CasesAvg7Days(float? Today, bool Sublabel, float? DiffPercentage, int Year, int Month, int Day)
        : SummaryBase(DiffPercentage, Year, Month, Day);
    public record HospitalizedCurrent(int? Total, int? In, int? Out, int? Deceased, float? DiffPercentage, int Year, int Month, int Day)
        : SummaryBase(DiffPercentage, Year, Month, Day);
    public record ICUCurrent(int? Total, int? In, int? Out, int? Deceased, float? DiffPercentage, int Year, int Month, int Day)
        : SummaryBase(DiffPercentage, Year, Month, Day);
    public record DeceasedToDate(int? Total, int? Deceased, float? DiffPercentage, int Year, int Month, int Day)
        : SummaryBase(DiffPercentage, Year, Month, Day);
}
