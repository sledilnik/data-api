using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record MunicipalityDay : IModelDate
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public ImmutableDictionary<string, ImmutableDictionary<string, MunicipalityDayData>> Regions { get; init; }
        public MunicipalityDay(int year, int month, int day, ImmutableDictionary<string, ImmutableDictionary<string, MunicipalityDayData>> regions)
        {
            Year = year;
            Month = month;
            Day = day;
            Regions = regions;
        }
    }

    public record MunicipalityDayData
    {
        public int? ActiveCases { get; init; }
        public int? ConfirmedToDate { get; init; }
        public int? DeceasedToDate { get; init; }
        public MunicipalityDayData(int? activeCases, int? confirmedToDate, int? deceasedToDate)
        {
            ActiveCases = activeCases;
            ConfirmedToDate = confirmedToDate;
            DeceasedToDate = deceasedToDate;
        }
    }
}
