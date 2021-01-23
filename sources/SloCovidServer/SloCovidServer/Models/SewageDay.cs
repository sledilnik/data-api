using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record SewageDay(int Year, int Month, int Day, ImmutableDictionary<string, SewageCityDay> Cities) { }
    public record SewageCityDay(ImmutableDictionary<string, float?> Measurements) { }
}
