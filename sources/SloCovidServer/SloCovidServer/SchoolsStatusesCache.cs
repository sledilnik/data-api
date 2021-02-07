using System.Collections.Immutable;
using SloCovidServer.Models;

namespace SloCovidServer
{
    public record SchoolsStatusesCache
    {
        public string SchoolAbsencesETag { get; }
        public string SchoolRegimesETag { get; }
        public ImmutableArray<SchoolAbsenceDay> Absences { get; }
        public ImmutableArray<SchoolRegimeDay> Regimes { get; }
        public string ETag { get; }
        public ImmutableDictionary<string, SchoolStatus> Value { get; }
        public SchoolsStatusesCache(string schoolAbsencesETag, string schoolRegimesETag, ImmutableArray<SchoolAbsenceDay> absences, ImmutableArray<SchoolRegimeDay> regimes,
            ImmutableDictionary<string, SchoolStatus> value)
        {
            SchoolAbsencesETag = schoolAbsencesETag;
            SchoolRegimesETag = schoolRegimesETag;
            Absences = absences;
            Regimes = regimes;
            Value = value;
            ETag = $"{SchoolAbsencesETag}:{SchoolRegimesETag}";
        }
    }
}
