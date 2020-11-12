using System;
using System.Collections.Immutable;

namespace SloCovidServer.Models.Owid
{
    public record CountryData(DateTime Date, string TestsUnits, ImmutableDictionary<string, float?> AllColumns);
}
