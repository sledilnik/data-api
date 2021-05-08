using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record VaccinationByAgeDay(int Year, int Month, int Day, ImmutableDictionary<string, VaccinationByAgeToDate> Ages);
    public record VaccinationByAgeToDate(int? First, int? Second);

}
