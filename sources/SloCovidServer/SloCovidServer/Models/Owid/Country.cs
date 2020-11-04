using System;
using System.Collections.Immutable;

namespace SloCovidServer.Models.Owid
{
    public class Country
    {
        public string Continent { get; }
        public string Location { get; }
        public float? Population { get; }
        public float? PopulationDensity { get; }
        public float? MedianAge {  get; }
        public float? Aged65Older { get; }
        public float? Aged70Older { get; }
        public float? CardiovascDeathRate { get; }
        public float? HandwashingFacilities { get; }
        public float? HospitalBedsPerThousand { get; }
        public float? GdpPerCapita { get; }
        public float? DiabetesPrevalence { get; }
        public float? LifeExpectancy { get; }
        public float? HumanDevelopmentIndex { get; }
        public ImmutableArray<CountryData> Data { get; }
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
