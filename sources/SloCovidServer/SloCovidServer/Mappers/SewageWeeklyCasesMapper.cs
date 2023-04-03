using System.Collections.Immutable;
using SloCovidServer.Models;

namespace SloCovidServer.Mappers;

public class SewageWeeklyCasesMapper : Mapper
{
    public ImmutableArray<SewageWeeklyCases> GetSewageWeeklyCasesFromRaw(string raw)
    {
        string[] lines = raw.Split('\n');
        var header = ParseHeader(lines[0]);
        int dateIndex = header["sewage.date"];
        int stationIndex = header["sewage.station"];
        int flowIndex = header["sewage.flow"];
        int n3RawIndex = header["sewage.n3-raw"];
        int codIndex = header["sewage.cod"];
        int n3NormIndex = header["sewage.n3-norm"];
        int estimatedCasesIndex = header["sewage.cases.estimated"];
        int activeCasesIndex = header["sewage.cases.active100k"];
        int latIndex = header["sewage.lat"];
        int lonIndex = header["sewage.lon"];
        int regionIndex = header["sewage.region"];
        int populationIndex = header["sewage.population"];
        int coverageRatioIndex = header["sewage.coverage-ratio"];
        var result = ImmutableArray.CreateBuilder<SewageWeeklyCases>();
        foreach (string line in IterateLines(lines))
        {
            var fields = ParseLine(line);
            var date = GetDate(fields[dateIndex]);
            var n3 = new SewageN3(
                GetFloat(fields[n3RawIndex]),
                GetFloat(fields[n3NormIndex])
            );
            var cases = new SewageCase(
                GetFloat(fields[estimatedCasesIndex]),
                GetFloat(fields[activeCasesIndex])
            );
            var item = new SewageWeeklyCases(
                date.Year, date.Month, date.Day,
                fields[stationIndex],
                GetInt(fields[flowIndex]),
                n3,
                GetInt(fields[codIndex]),
                cases,
                GetFloat(fields[latIndex]),
                GetFloat(fields[lonIndex]),
                fields[regionIndex],
                GetInt(fields[populationIndex]),
                GetFloat(fields[coverageRatioIndex])
                );
            result.Add(item);
        }
        return result.ToImmutable();
    }
}
