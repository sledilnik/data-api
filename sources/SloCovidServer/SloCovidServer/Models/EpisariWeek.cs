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
        public int? CovidInVaccinationYes { get; init; }
        public int? CovidInVaccinationNo { get; init; }
        public int? CovidInVaccinationUnknown { get; init; }
        public int? CovidDiscoveredInHospital { get; init; }
        public int? CovidAcquiredInHospital { get; init; }
        public int? CovidDeceased { get; init; }
        public int? CovidIcuIn { get; init; }
        public ImmutableDictionary<string, EpisariPerAge> PerAge { get; init; }
        public EpisariWeek(string week, Date from, Date to)
        {
            Week = week;
            From = from;
            To = to;
        }
    }
    public record EpisariPerAge(int? CovidIn, int? Vaccination, int? Deceased, int? IcuIn)
    {
        public static readonly EpisariPerAge Empty = new EpisariPerAge(default, default, default, default);
    }
}
