using SloCovidServer.Models;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Services.Abstract
{
    public interface ICommunicator
    {
        Task<(ImmutableArray<StatsDaily>? Data, string ETag, long? Timestamp)> GetStatsAsync(string callerEtag, CancellationToken ct);
        Task<(ImmutableArray<RegionsDay>? Data, string ETag, long? Timestamp)> GetRegionsAsync(string callerEtag, CancellationToken ct);
        Task<(ImmutableArray<PatientsDay>? Data, string ETag, long? Timestamp)> GetPatientsAsync(string callerEtag, CancellationToken ct);
        Task<(ImmutableArray<HospitalsDay>? Data, string ETag, long? Timestamp)> GetHospitalsAsync(string callerEtag, CancellationToken ct);
        Task<(ImmutableArray<Hospital>? Data, string ETag, long? Timestamp)> GetHospitalsListAsync(string callerEtag, CancellationToken ct);
        Task<(ImmutableArray<Municipality>? Data, string ETag, long? Timestamp)> GetMunicipalitiesListAsync(string callerEtag, CancellationToken ct);
        Task<(ImmutableArray<RetirementHome>? Data, string ETag, long? Timestamp)> GetRetirementHomesListAsync(string callerEtag, CancellationToken ct);
        Task<(ImmutableArray<RetirementHomesDay>? Data, string ETag, long? Timestamp)> GetRetirementHomesAsync(string callerEtag, CancellationToken ct);
        Task<(ImmutableArray<DeceasedPerRegionsDay>? Data, string ETag, long? Timestamp)> GetDeceasedPerRegionsAsync(string callerEtag, CancellationToken ct);
        Task<(ImmutableArray<MunicipalityDay>? Data, string ETag, long? Timestamp)> GetMunicipalitiesAsync(string callerEtag, CancellationToken ct);
        Task<(ImmutableArray<HealthCentersDay>? Data, string ETag, long? Timestamp)> GetHealthCentersAsync(string callerEtag, CancellationToken ct);
    }
}
