﻿using SloCovidServer.Models;
using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Services.Abstract
{
    public interface ICommunicator
    {
        Task InitialCacheRefreshAsync(CancellationToken ct);
        Task StartCacheRefresherAsync(CancellationToken ct);
        Task<(ImmutableArray<StatsDaily>? Data, string raw, string ETag, long? Timestamp)?> GetStatsAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<PatientsDay>? Data, string raw, string ETag, long? Timestamp)?> GetPatientsAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<HospitalsDay>? Data, string raw, string ETag, long? Timestamp)?> GetHospitalsAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<Hospital>? Data, string raw, string ETag, long? Timestamp)?> GetHospitalsListAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<Municipality>? Data, string raw, string ETag, long? Timestamp)?> GetMunicipalitiesListAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<RetirementHome>? Data, string raw, string ETag, long? Timestamp)?> GetRetirementHomesListAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<RetirementHomesDay>? Data, string raw, string ETag, long? Timestamp)?> GetRetirementHomesAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<MunicipalityDay>? Data, string raw, string ETag, long? Timestamp)?> GetMunicipalitiesAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<RegionCasesDay>? Data, string raw, string ETag, long? Timestamp)?> GetRegionCasesAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<HealthCentersDay>? Data, string raw, string ETag, long? Timestamp)?> GetHealthCentersAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<StatsWeeklyDay>? Data, string raw, string ETag, long? Timestamp)?> GetStatsWeeklyAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        (ImmutableDictionary<string, Models.Owid.Country> Data, string raw, string eTag)? GetOwidCountries(string callerEtag);
        Task<(ImmutableArray<MonthlyDeathsSlovenia>? Data, string raw, string ETag, long? Timestamp)?> GetMonthlyDeathsSloveniaAsync(string callerEtag,
            DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<LabTestDay>? Data, string raw, string ETag, long? Timestamp)?> GetLabTestsAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<DailyDeathsSlovenia>? Data, string raw, string ETag, long? Timestamp)?> GetDailyDeathsSloveniaAsync(string callerEtag,
            DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<AgeDailyDeathsSloveniaDay>? Data, string raw, string ETag, long? Timestamp)?> GetAgeDailyDeathsSloveniaAsync(string callerEtag,
            DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<SewageDay>? Data, string raw, string ETag, long? Timestamp)?> GetSewageAsync(string callerEtag,
            DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<SchoolCasesDay>? Data, string raw, string ETag, long? Timestamp)?> GetSchoolCasesAsync(string callerEtag,
            DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<VaccinationDay>? Data, string raw, string ETag, long? Timestamp)?> GetVaccinationsAsync(string callerEtag,
        DataFilter filter, CancellationToken ct); Task<(ImmutableArray<SewageWeeklyCases>? Data, string raw, string ETag, long? Timestamp)?> 
            GetSewageWeeklyCasesAsync(string callerEtag, DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<SewageGenomeDay>? Data, string raw, string ETag, long? Timestamp)?> GetSewageGenomesAsync(string callerEtag,
            DataFilter filter, CancellationToken ct);
        Task<(ImmutableArray<EpisariWeek>? Data, string raw, string ETag, long? Timestamp)?> GetEpisariWeeksAsync(string callerEtag,
            DataFilter filter, CancellationToken ct);
        (Summary Summary, string ETag)? GetSummary(string callerEtag, DateTime? toDate);

        Task<(ImmutableArray<OpsiCase>? Data, string raw, string ETag, long? Timestamp)?> GetOpsiCasesAsync(
            string callerEtag,
            DataFilter filter, CancellationToken ct);
        (ImmutableDictionary<string, SchoolStatus> Summary, string ETag)? GetSchoolsStatuses(string callerEtag, SchoolsStatusesFilter filter);
    }
}
