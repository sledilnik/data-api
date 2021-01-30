using SloCovidServer.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class SchoolCasesMapper : Mapper
    {
        public ImmutableArray<SchoolCasesDay> GetFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var result = new List<SchoolCasesDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var cases = ImmutableDictionary<string, ImmutableDictionary<string, ImmutableDictionary<string, int>>>.Empty;
                var query = from pair in header
                            let parts = pair.Key.Split('.')
                            where parts.Length == 3
                            let v = GetInt(fields[pair.Value])
                            where v.HasValue
                            select new { SchoolType = parts[0], PersonType = parts[1], CaseType = parts[2], Cases = v };
                foreach (var c in query)
                {
                    if (!cases.TryGetValue(c.SchoolType, out var schoolTypes))
                    {
                        schoolTypes = ImmutableDictionary<string, ImmutableDictionary<string, int>>.Empty;
                    }
                    if (!schoolTypes.TryGetValue(c.PersonType, out var personTypes))
                    {
                        personTypes = ImmutableDictionary<string, int>.Empty;
                    }
                    personTypes = personTypes.SetItem(c.CaseType, c.Cases.Value);
                    schoolTypes = schoolTypes.SetItem(c.PersonType, personTypes);
                    cases = cases.SetItem(c.SchoolType, schoolTypes);
                }

                var date = GetDate(fields[dateIndex]);
                result.Add(new SchoolCasesDay(
                    date.Year,
                    date.Month,
                    date.Day,
                    cases
                ));
            }
            return result.ToImmutableArray();
        }
    }
}
