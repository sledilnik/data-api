using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record EpisariWeek
    {
        public string Week { get; init; }
        public Date To { get; init; }
        public Date From { get; init; }
        public string Source { get; init; }
        public string Missing {  get; init; }
        public int? SariIn { get; init; }
        public int? TestedIn { get; init; }
        public int? CovidIn { get; init; }
        public int? CovidOut { get; init; }
        public int? CovidInNotSari { get; init; }
        public int? CovidInVaccinated { get; init; }
        public int? CovidInVaccinatedUnknown { get; init; }
        public int? CovidInNotVaccinated { get; init; }
        public int? CovidDiscoveredInHospital { get; init; }
        public int? CovidAcquiredInHospital { get; init; }
        public int? CovidDeceased { get; init; }
        public int? CovidIcuIn { get; init; }
        public int? CovidInVaccinated3 { get; init; }
        public int? CovidInVaccinatedFull { get; init; }
        public int? CovidInVaccinatedPartial { get; init; }
        public int? CovidInVaccinatedNone { get; init; }
        public ImmutableArray<EpisariPerAgeBucket> PerAge { get; init; }
        public EpisariWeek(string week, Date from, Date to)
        {
            Week = week;
            From = from;
            To = to;
        }
    }
    public record EpisariPerAge(int? CovidIn, int? VaccinatedIn, int? IcuIn, int? Deceased, int? VaccinatedIn3, int? VaccinatedInFull)
    {
        public static readonly EpisariPerAge Empty = new EpisariPerAge(default, default, default, default, default, default);
    }
    public record EpisariPerAgeBucket(int? AgeFrom, int? AgeTo, int? CovidIn, int? VaccinatedIn, int? IcuIn, int? Deceased, int? VaccinatedIn3, int? VaccinatedInFull);
}
