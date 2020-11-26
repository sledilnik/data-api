using SloCovidServer.Models;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class MonthlyDeathsSloveniaMapper : Mapper
    {
        public ImmutableArray<MonthlyDeathsSlovenia> GetFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int yearIndex = header["year"];
            int monthIndex = header["month"];
            int deceasedIndex = header["deceased"];
            var query = from l in IterateLines(lines)
                        let fields = ParseLine(l)
                        select new MonthlyDeathsSlovenia
                        {
                            Year = GetInt(fields[yearIndex]).Value,
                            Month = GetInt(fields[monthIndex]).Value,
                            Deceased = GetInt(fields[deceasedIndex]).Value
                        };
            return query.ToImmutableArray();
        }
    }
}
