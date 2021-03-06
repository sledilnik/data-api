﻿using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record VaccinationDay(int Year, int Month, int Day, VaccinationData Administered, VaccinationData Administered2nd, int? UsedToDate, int? DeliveredToDate,
        ImmutableDictionary<string, int> DeliveredByManufacturer): IModelDate;
    public record VaccinationData(int? Today, int? ToDate);
}
