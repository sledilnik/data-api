using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record RegionCasesDay : IModelDate
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public ImmutableDictionary<string, RegionCasesDayData> Regions { get; init; }
        public RegionCasesDay(int year, int month, int day, ImmutableDictionary<string, RegionCasesDayData> regions)
        {
            Year = year;
            Month = month;
            Day = day;
            Regions = regions;
        }
    }

    public record RegionCasesDayData
    {
        public int? ActiveCases { get; init; }
        public int? ConfirmedToDate { get; init; }
        public int? DeceasedToDate { get; init; }
        public RegionCasesDayData(int? activeCases, int? confirmedToDate, int? deceasedToDate)
        {
            ActiveCases = activeCases;
            ConfirmedToDate = confirmedToDate;
            DeceasedToDate = deceasedToDate;
        }
    }
}
