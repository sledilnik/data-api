using System.Collections.Immutable;

namespace SloCovidServer.Models.Owid
{
    public record Country(ImmutableDictionary<string, object> AllColumns, ImmutableArray<CountryData> Data);
}
