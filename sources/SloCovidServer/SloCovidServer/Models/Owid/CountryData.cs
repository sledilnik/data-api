using System;

namespace SloCovidServer.Models.Owid
{
    public record CountryData
    {
        public DateTime Date { get; init; }
        public float? TotalCases { get; init; }
        public float? NewCases { get; init; }
        public float? NewDeathsSmoothed { get; init; }
        public float? TotalDeaths { get; init; }
        public float? NewDeaths { get; init; }
        public float? NewCasesSmoothed { get; init; }
        public float? TotalCasesPerMillion { get; init; }
        public float? NewCasesPerMillion { get; init; }
        public float? NewCasesSmoothedPerMillion { get; init; }
        public float? TotalDeathsPerMillion { get; init; }
        public float? NewDeathsPerMillion { get; init; }
        public float? NewDeathsSmoothedPerMillion { get; init; }
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
