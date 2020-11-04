using System;

namespace SloCovidServer.Models.Owid
{
    public class CountryData
    {
        public DateTime Date { get; }
        public float? TotalCases { get; }
        public float? NewCases { get; }
        public float? NewDeathsSmoothed { get; }
        public float? TotalDeaths { get; }
        public float? NewDeaths { get; }
        public float? NewCasesSmoothed { get; }
        public float? TotalCasesPerMillion { get; }
        public float? NewCasesPerMillion { get; }
        public float? NewCasesSmoothedPerMillion { get; }
        public float? TotalDeathsPerMillion { get; }
        public float? NewDeathsPerMillion { get; }
        public float? NewDeathsSmoothedPerMillion { get; }
        public CountryData(DateTime date, float? totalCases, float? newCases, float? newDeathsSmoothed, float? totalDeaths, 
            float? newDeaths, float? newCasesSmoothed, float? totalCasesPerMillion, float? newCasesPerMillion, 
            float? newCasesSmoothedPerMillion, float? totalDeathsPerMillion, float? newDeathsPerMillion, float? newDeathsSmoothedPerMillion)
        {
            Date = date;
            TotalCases = totalCases;
            NewCases = newCases;
            NewDeathsSmoothed = newDeathsSmoothed;
            TotalDeaths = totalDeaths;
            NewDeaths = newDeaths;
            NewCasesSmoothed = newCasesSmoothed;
            TotalCasesPerMillion = totalCasesPerMillion;
            NewCasesPerMillion = newCasesPerMillion;
            NewCasesSmoothedPerMillion = newCasesSmoothedPerMillion;
            TotalDeathsPerMillion = totalDeathsPerMillion;
            NewDeathsPerMillion = newDeathsPerMillion;
            NewDeathsSmoothedPerMillion = newDeathsSmoothedPerMillion;
        }

    }
}
