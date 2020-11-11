using System.Collections.Generic;
using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record RegionsDay : IModelDate
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public Dictionary<string, Dictionary<string, int?>> Regions { get; init; }
        public RegionsDay(int year, int month, int day, Dictionary<string, Dictionary<string, int?>> regions)
        {
            Year = year;
            Month = month;
            Day = day;
            Regions = regions;
        }

        public int? FindByPlace(string place)
        {
            foreach (var region in Regions)
            {
                if (region.Value.TryGetValue(place, out int? result))
                {
                    return result;
                }
            }
            return null;
        }
    }

    public record RegionSum
    {
        public string Code { get; init; }
        public string Name { get; init; }
        public string AltName { get; init; }
        public ImmutableArray<MunicipalitySum> Municipalities { get; init; }
        public RegionSum(string code, string name, string altName, ImmutableArray<MunicipalitySum> municipalities)
        {
            Code = code;
            Name = name;
            AltName = altName;
            Municipalities = municipalities;
        }
    }

    public record MunicipalitySum
    {
        public string Code { get; init; }
        public string Name { get; init; }
        public string AltName { get; init; }
        public int? Value { get; init; }
        public MunicipalitySum(string code, string name, string altName, int? value)
        {
            Code = code;
            Name = name;
            AltName = altName;
            Value = value;
        }
    }
}
