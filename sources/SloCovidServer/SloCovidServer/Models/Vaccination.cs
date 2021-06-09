using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record VaccinationDay(int Year, int Month, int Day,
        VaccinationData Administered, VaccinationData Administered2nd,
        int? UsedToDate, ImmutableDictionary<string, int> UsedByManufacturer,
        int? DeliveredToDate, ImmutableDictionary<string, int> DeliveredByManufacturer,
        ImmutableArray<PerAgeBucket> administeredPerAge): IModelDate;
    public record VaccinationData(int? Today, int? ToDate);
}
