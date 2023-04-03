using System.Collections.Immutable;
using SloCovidServer.Models;

namespace SloCovidServer.Mappers
{
    public class SewageGenomesMapper : Mapper
    {
        public ImmutableArray<SewageGenomeDay> GetSewageGenomesFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["sewage.date"];
            int stationIndex = header["sewage.station"];
            int genomeIndex = header["sewage.genome"];
            int ratioIndex = header["sewage.ratio"];
            int regionIndex = header["sewage.region"];
            var result = ImmutableArray.CreateBuilder<SewageGenomeDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var date = GetDate(fields[dateIndex]);
                var item = new SewageGenomeDay(
                    date.Year, date.Month, date.Day,
                    fields[stationIndex],
                    fields[genomeIndex],
                    GetFloat(fields[ratioIndex]),
                    fields[regionIndex]
                    );
                result.Add(item);
            }
            return result.ToImmutable();
        }
    }
}
