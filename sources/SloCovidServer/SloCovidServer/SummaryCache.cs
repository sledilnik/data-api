using SloCovidServer.Models;
using System.Collections.Immutable;

namespace SloCovidServer.Services.Implemented
{
    public record SummaryCache
    {
        public string StatsETag { get; }
        public string PatientsETag { get; }
        public string LabTestsETag { get; }
        public ImmutableArray<StatsDaily> Stats { get; }
        public ImmutableArray<PatientsDay> Patients { get; }
        public ImmutableArray<LabTestDay> LabTests { get; }
        public string ETag { get; }
        public Summary Value { get; }
        public SummaryCache(string statsETag, ImmutableArray<StatsDaily> stats, string patientsETag, ImmutableArray<PatientsDay> patients, string labTestsETag, ImmutableArray<LabTestDay> labTests,Summary value)
        {
            StatsETag = statsETag;
            Stats = stats;
            PatientsETag = patientsETag;
            Patients = patients;
            LabTestsETag = labTestsETag;
            LabTests = labTests;
            Value = value;
            ETag = $"{StatsETag}:{PatientsETag}:{LabTestsETag}";
        }
    }
}
