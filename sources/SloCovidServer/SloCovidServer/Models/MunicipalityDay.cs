using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public class MunicipalityDay
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public ImmutableDictionary<string, ImmutableDictionary<string, MunicipalityDayData>> Regions { get; }
        public MunicipalityDay(int year, int month, int day, ImmutableDictionary<string, ImmutableDictionary<string, MunicipalityDayData>> regions)
        {
            Year = year;
            Month = month;
            Day = day;
            Regions = regions;
        }
    }

    public class MunicipalityDayData
    {
        public int? ConfirmedToDate { get; }
        public int? DeceasedToDate { get; }
        public MunicipalityDayData(int? confirmedToDate, int? deceasedToDate)
        {
            ConfirmedToDate = confirmedToDate;
            DeceasedToDate = deceasedToDate;
        }
    }
}
