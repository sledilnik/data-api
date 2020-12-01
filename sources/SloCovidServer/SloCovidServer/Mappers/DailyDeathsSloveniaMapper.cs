using SloCovidServer.Models;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class DailyDeathsSloveniaMapper : Mapper
    {
        public ImmutableArray<DailyDeathsSlovenia> GetFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            int deceasedIndex = header["deceased"];
            var query = from l in IterateLines(lines)
                        let fields = ParseLine(l)
                        let date = GetDate(fields[dateIndex])
                        select new DailyDeathsSlovenia
                        {
                            Year = date.Year,
                            Month = date.Month,
                            Day = date.Day,
                            Deceased = GetInt(fields[deceasedIndex]).Value
                        };
            return query.ToImmutableArray();
        }
    }
}
