using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record LabTestData(TodayToDate Performed, TodayToDate Positive)
    {
        public static LabTestData Empty { get; } = new LabTestData(TodayToDate.Empty, TodayToDate.Empty);
    }
    public record LabTestDay(int Year, int Month, int Day, LabTestData Total, ImmutableDictionary<string, LabTestData> Data, ImmutableDictionary<string, LabTestData> Labs) : IModelDate;
}