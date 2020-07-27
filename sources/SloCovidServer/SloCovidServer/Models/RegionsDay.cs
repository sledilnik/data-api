using System.Collections.Generic;
using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public class RegionsDay: IModelDate
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public Dictionary<string, Dictionary<string, int?>> Regions { get; }
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

    public class RegionSum
    {
        public string Code { get; }
        public string Name { get; }
        public string AltName { get; }
        public ImmutableArray<MunicipalitySum> Municipalities { get; }
        public RegionSum(string code, string name, string altName, ImmutableArray<MunicipalitySum> municipalities)
        {
            Code = code;
            Name = name;
            AltName = altName;
            Municipalities = municipalities;
        }
    }

    public class MunicipalitySum
    {
        public string Code { get; }
        public string Name { get; }
        public string AltName { get; }
        public int? Value { get; }
        public MunicipalitySum(string code, string name, string altName, int? value)
        {
            Code = code;
            Name = name;
            AltName = altName;
            Value = value;
        }
    }
}
