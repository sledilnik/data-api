using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SloCovidServer.Models
{
    public record StatsWeeklyDay : IModelDate
    {
        public string Week { get; init; }
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public Date To { get; init; }
        public int? HospitalizedVaccinated { get; init; }
        public int? HospitalizedOther { get; init; }
        public int? IcuVaccinated { get; init; }
        public int? IcuVaccinatedPartially { get; init; }
        public int? IcuRecovered { get; init; }
        public int? IcuOther { get; init; }
        public int? Confirmed { get; init; }
        public int? Investigated { get; init; }
        public int? Healthcare { get; init; }
        public int? HealthcareMale { get; init; }
        public int? HealthcareFemale { get; init; }
        [JsonPropertyName("rh-occupant")]
        public int? RhOccupant { get; init; }
        public StatsWeeklySentTo SentTo { get; init; }
        public ImmutableDictionary<string, int?> Source { get; init; }
        public ImmutableDictionary<string, int?> From { get; init; }
        public ImmutableDictionary<string, int?> Locations { get; init; }
        public StatsWeeklyDay(string week, int year, int month, int day, Date to,
            int? hospitalizedVaccinated, int? hospitalizedOther,
            int? icuVaccinated, int? icuVaccinatedPartially, int? icuRecovered, int? icuOther,
            int? confirmed, int? investigated,
            int? healthcare, int? healthcareMale, int? healthcareFemale,int? rhOccupant,
            StatsWeeklySentTo sentTo,
            ImmutableDictionary<string, int?> source,ImmutableDictionary<string, int?> from, ImmutableDictionary<string, int?> locations)
        {
            Week = week;
            Year = year;
            Month = month;
            Day = day;
            To = to;
            HospitalizedVaccinated = hospitalizedVaccinated;
            HospitalizedOther = hospitalizedOther;
            IcuVaccinated = icuVaccinated;
            IcuVaccinatedPartially = icuVaccinatedPartially;
            IcuRecovered = icuRecovered;
            IcuOther = icuOther;
            Confirmed = confirmed;
            Investigated = investigated;
            Healthcare = healthcare;
            HealthcareMale = healthcareMale;
            HealthcareFemale = healthcareFemale;
            RhOccupant = rhOccupant;
            SentTo = sentTo;
            Source = source;
            From = from;
            Locations = locations;
        }
    }

    public record StatsWeeklySentTo
    {
        public int? Quarantine { get; init; }
        public StatsWeeklySentTo(int? quarantine)
        {
            Quarantine = quarantine;
        }
    }
}
