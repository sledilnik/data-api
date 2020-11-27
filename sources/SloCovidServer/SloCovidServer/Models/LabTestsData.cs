using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record LabTestData(ToDateToday Performed, ToDateToday Positive)
    {
        public static LabTestData Empty { get; } = new LabTestData(ToDateToday.Empty, ToDateToday.Empty);
    }
    public record LabTestDay(int Year, int Month, int Day, LabTestData Total, ImmutableDictionary<string, LabTestData> Data, ImmutableDictionary<string, LabTestData> Labs) : IModelDate;
}