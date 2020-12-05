using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record AgeDailyDeathsSloveniaDay(int Year, int Month, int Day, 
        ImmutableDictionary<string, int?> Male, ImmutableDictionary<string, int?> Female) : IModelDate;
}
