using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public class DeceasedPerRegionsDay : IModelDate
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public ImmutableDictionary<string, ImmutableDictionary<string, int?>> Regions { get; }
        public DeceasedPerRegionsDay(int year, int month, int day, ImmutableDictionary<string, ImmutableDictionary<string, int?>> regions)
        {
            Year = year;
            Month = month;
            Day = day;
            Regions = regions;
        }
    }
}
