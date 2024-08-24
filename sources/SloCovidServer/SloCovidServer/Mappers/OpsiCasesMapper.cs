using System.Collections.Generic;
using System.Collections.Immutable;
using SloCovidServer.Models;

namespace SloCovidServer.Mappers;

public class OpsiCasesMapper: Mapper
{
    public ImmutableArray<OpsiCase> GetFromRaw(string raw)
    {
        string[] lines = raw.Split('\n');
        var header = ParseHeader(lines[0]);
        int yearWeekIndex = header["TheISOYearWeek"];
        int dateIndex = header["TheDate"];
        int testPcrIndex = header["st_test_PCR"];
        int testHagtIndex = header["st_test_HAGT"];
        int confirmedCasesIndex = header["st_potrjenih_primerov"];
        var result = new List<OpsiCase>(lines.Length);
        foreach (string line in IterateLines(lines))
        {
            var fields = ParseLine(line);
            var yearWeekText = fields[yearWeekIndex];
            var yearWeek = new YearWeek(int.Parse(yearWeekText[..4]), int.Parse(yearWeekText[5..7]));
            var date = GetDate(fields[dateIndex]);
            var testPcr = GetInt(fields[testPcrIndex])!.Value;
            var testHagt = GetInt(fields[testHagtIndex])!.Value;
            var confirmedCases = GetInt(fields[confirmedCasesIndex])!.Value;
            result.Add(new OpsiCase(yearWeek, date.Year, date.Month, date.Day, testPcr, testHagt, confirmedCases));
        }
        return [..result];
    }
}