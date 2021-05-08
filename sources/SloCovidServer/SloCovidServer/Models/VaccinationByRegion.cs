using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record VaccinationByRegionDay(int Year, int Month, int Day, ImmutableDictionary<string, VaccinationByRegionDayData> Regions);
    public record VaccinationByRegionDayData(TodayToDate First, TodayToDate Second);
}
