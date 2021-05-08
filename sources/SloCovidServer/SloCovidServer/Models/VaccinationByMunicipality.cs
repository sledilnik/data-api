using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Dictionary levels are regions and municipality respectively.</remarks>
    public record VaccinationByMunicipalityDay(int Year, int Month, int Day, 
        ImmutableDictionary<string, ImmutableDictionary<string, VaccinationByMunicipalityToDate>> Regions);
    public record VaccinationByMunicipalityToDate(int? First, int? Second);
}
