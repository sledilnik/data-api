using System.Collections.Immutable;

namespace SloCovidServer.Models.Owid
{
    public record Country
    {
        public string Continent { get; init; }
        public string Location { get; init; }
        public float? Population { get; init; }
        public float? PopulationDensity { get; init; }
        public float? MedianAge { get; init; }
        public float? Aged65Older { get; init; }
        public float? Aged70Older { get; init; }
        public float? CardiovascDeathRate { get; init; }
        public float? HandwashingFacilities { get; init; }
        public float? HospitalBedsPerThousand { get; init; }
        public float? GdpPerCapita { get; init; }
        public float? DiabetesPrevalence { get; init; }
        public float? LifeExpectancy { get; init; }
        public float? HumanDevelopmentIndex { get; init; }
        public ImmutableArray<CountryData> Data { get; init; }
        public Country(string continent, string location, float? population, float? populationDensity, float? medianAge, float? aged65Older,
            float? aged70Older, float? cardiovascDeathRate, float? handwashingFacilities, float? hospitalBedsPerThousand,
            float? gdpPerCapita, float? diabetesPrevalence, float? lifeExpectancy, float? humanDevelopmentIndex, ImmutableArray<CountryData> data)
        {
            Continent = continent;
            Location = location;
            Population = population;
            PopulationDensity = populationDensity;
            MedianAge = medianAge;
            Aged65Older = aged65Older;
            Aged70Older = aged70Older;
            CardiovascDeathRate = cardiovascDeathRate;
            HandwashingFacilities = handwashingFacilities;
            HospitalBedsPerThousand = hospitalBedsPerThousand;
            GdpPerCapita = gdpPerCapita;
            DiabetesPrevalence = diabetesPrevalence;
            LifeExpectancy = lifeExpectancy;
            HumanDevelopmentIndex = humanDevelopmentIndex;
            Data = data;
        }

    }
}
