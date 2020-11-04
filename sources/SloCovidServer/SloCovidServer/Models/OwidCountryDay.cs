using System;

namespace SloCovidServer.Models
{
    public class OwidCountryDay
    {
        public DateTime Date { get; }
        public string IsoCode { get; }
        public float? NewCases { get; }
        public float? NewCasesPerMillion { get; }
        public float? TotalCases { get; }
        public float? TotalCasesPerMillion { get; }
        public float? TotalDeaths { get; }
        public float? TotalDeathsPerMillion { get; }
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
