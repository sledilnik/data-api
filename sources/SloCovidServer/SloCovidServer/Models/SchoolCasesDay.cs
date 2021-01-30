using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record SchoolCasesDay(int Year, int Month, int Day, ImmutableDictionary<string, ImmutableDictionary<string, ImmutableDictionary<string, int>>> SchoolType) { }
}
