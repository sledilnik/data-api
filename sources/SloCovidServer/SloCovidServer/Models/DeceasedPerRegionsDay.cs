using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record DeceasedPerRegionsDay : IModelDate
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public ImmutableDictionary<string, ImmutableDictionary<string, int?>> Regions { get; init; }
        public DeceasedPerRegionsDay(int year, int month, int day, ImmutableDictionary<string, ImmutableDictionary<string, int?>> regions)
        {
            Year = year;
            Month = month;
            Day = day;
            Regions = regions;
        }
    }
}
