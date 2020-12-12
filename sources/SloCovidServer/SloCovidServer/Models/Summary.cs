namespace SloCovidServer.Models
{
    public record Summary(
        CasesToDateSummary CasesToDateSummary, 
        CasesCurrent CasesCurrent,
        CasesAvg7Days CasesAvg7Days,
        HospitalizedCurrent HospitalizedCurrent,
        ICUCurrent ICUCurrent,
        DeceasedToDate DeceasedToDate);
    public record CasesToDateSummary(int? Number, int? In, float? DiffPercentage, int Year, int Month, int Date);
    public record CasesCurrent(int? Total, int? In, int? Out, int? Deceased, float? DiffPercentage);
    public record CasesAvg7Days(int? Today, bool Sublabel, float? DiffPercentage);
    public record HospitalizedCurrent(int? Total, int? In, int? Out, int? Deceased, float? DiffPercentage);
    public record ICUCurrent(int? Total, int? In, int? Out, int? Deceased, float? DiffPercentage);
    public record DeceasedToDate(int? Total, float? DiffPercentage);
}
