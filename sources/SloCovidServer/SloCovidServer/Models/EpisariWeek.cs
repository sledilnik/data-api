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
        public EpisariValueIn Sari { get; init; }
        public EpisariValueIn Tested { get; init; }
        public EpisariCovid Covid { get; init; }
        public EpisariWeek(string week, Date from, Date to)
        {
            Week = week;
            From = from;
            To = to;
        }
    }
    public record EpisariCovid
    {
        public EpisariCovidIn In { get; init; }
        public int? DiscoveredInHospital { get; init; }
        public int? AcquiredInHospital { get; init; }
        public EpisariDeceased Deceased { get; init; }
        public EpisariValueOut Out { get; init; }
        public EpisariIcu Icu {  get; init; }
    }
    public record EpisariIcu
    {
        public EpisariIcuIn In { get; init; }
    }
    public record EpisariIcuIn
    {
        public int? Value { get; init; }
        public ImmutableDictionary<string, int> PerAge { get; init; }
    }
    public record EpisariDeceased
    {
        public int? Value { get; init; }
        public ImmutableDictionary<string, int> PerAge { get; init; }
    }
    public record EpisariCovidIn
    {
        public int? Value { get; init; }
        public EpisariVaccinationStatus Vaccination { get; init; }
        public int? NotSari { get; init; }
        public ImmutableDictionary<string, int> PerAge { get; init; }

    }
    public record EpisariVaccinationStatus
    {
        public int? Yes { get; init; }
        public int? No { get; init; }
        public int? Unknown { get; init; }
        public ImmutableDictionary<string, int> PerAge { get; init; }
    }
    public record EpisariValueIn(EpisariValue In);
    public record EpisariValueOut(EpisariValue Out);
    public record EpisariValue(int? Value);

}
