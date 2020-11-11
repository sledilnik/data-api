using System;

namespace SloCovidServer.Models
{
    public record OwidCountryDay
    {
        public DateTime Date { get; init; }
        public string IsoCode { get; init; }
        public float? NewCases { get; init; }
        public float? NewCasesPerMillion { get; init; }
        public float? TotalCases { get; init; }
        public float? TotalCasesPerMillion { get; init; }
        public float? TotalDeaths { get; init; }
        public float? TotalDeathsPerMillion { get; init; }
        public OwidCountryDay(DateTime date, string isoCode, float? newCases, float? newCasesPerMillion, float? totalCases, float? totalCasesPerMillion,
            float? totalDeaths, float? totalDeathsPerMillion)
        {
            Date = date;
            IsoCode = isoCode;
            NewCases = newCases;
            NewCasesPerMillion = newCasesPerMillion;
            TotalCases = totalCases;
            TotalCasesPerMillion = totalCasesPerMillion;
            TotalDeaths = totalDeaths;
            TotalDeathsPerMillion = totalDeathsPerMillion;
        }
    }
}
