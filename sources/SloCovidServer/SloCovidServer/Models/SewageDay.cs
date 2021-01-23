using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record SewageDay(int Year, int Month, int Day, ImmutableDictionary<string, ImmutableDictionary<string, float?>> Plants) { }
}
