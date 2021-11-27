using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record EpisariWeek
    {
        public string Week { get; init; }
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public Date To { get; init; }
        public string Source { get; init; }
        public string Missing {  get; init; }
        public int? SariIn { get; init; }
        public int? TestedIn { get; init; }
        public int? CovidIn { get; init; }
        public int? CovidInNotSari { get; init; }
        public int? CovidInVaccinated { get; init; }
        public int? CovidInVaccinatedUnknown { get; init; }
        public int? CovidInNotVaccinated { get; init; }
        public int? CovidIcuIn { get; init; }
        public int? CovidDiscoveredInHospital { get; init; }
        public int? CovidAcquiredInHospital { get; init; }
        public int? CovidOut { get; init; }
        public int? CovidDeceased { get; init; }
        public ImmutableArray<EpisariPerAgeBucket> PerAge { get; init; }
        public EpisariWeek(string week, Date from, Date to)
        {
            Week = week;
            Year = from.Year;
            Month = from.Month;
            Day = from.Day;
            To = to;
        }
    }
    public record EpisariPerAge(int? CovidIn, int? VaccinatedIn, int? IcuIn, int? Deceased)
    {
        public static readonly EpisariPerAge Empty = new EpisariPerAge(default, default, default, default);
    }
    public record EpisariPerAgeBucket(int? AgeFrom, int? AgeTo, int? CovidIn, int? VaccinatedIn, int? IcuIn, int? Deceased);
}
