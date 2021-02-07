using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SloCovidServer.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public class SchoolsMapper : Mapper
    {
        public ImmutableArray<SchoolCasesDay> GetCasesFromRaw(string raw)
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
        public ImmutableArray<SchoolAbsenceDay> GetAbsencesFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            int absentFromIndex = header["absent.from"];
            int absentToIndex = header["absent.to"];
            int schoolTypeIndex = header["school_type"];
            int schoolIndex = header["school"];
            int personTypeIndex = header["person_type"];
            int personClassIndex = header["person_class"];
            int reasonIndex = header["reason"];
            var result = new List<SchoolAbsenceDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var date = GetDate(fields[dateIndex]);
                var model = new SchoolAbsenceDay(date.Year, date.Month, date.Day, 
                    AbsentFrom: GetDate(fields[absentFromIndex]),
                    AbsentTo: GetDate(fields[absentToIndex]),
                    SchoolType: fields[schoolTypeIndex],
                    School: fields[schoolIndex],
                    PersonType: fields[personTypeIndex],
                    PersonClass: fields[personClassIndex],
                    Reason: fields[reasonIndex]
                );
                result.Add(model);
            }
            return result.ToImmutableArray();
        }
        public ImmutableArray<SchoolRegimeDay> GetRegimesFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            int changedFromIndex = header["changed.from"];
            int changedToIndex = header["changed.to"];
            int schoolTypeIndex = header["school_type"];
            int schoolIndex = header["school"];
            int personClassIndex = header["person_class"];
            int attendeesIndex = header["attendees"];
            int regimeIndex = header["regime"];
            int reasonIndex = header["reason"];
            var result = new List<SchoolRegimeDay>();
            foreach (string line in IterateLines(lines))
            {
                var fields = ParseLine(line);
                var date = GetDate(fields[dateIndex]);
                var model = new SchoolRegimeDay(date.Year, date.Month, date.Day,
                    ChangedFrom: GetDate(fields[changedFromIndex]),
                    ChangedTo: GetDate(fields[changedToIndex]),
                    SchoolType: fields[schoolTypeIndex],
                    School: fields[schoolIndex],
                    PersonClass: fields[personClassIndex],
                    Attendees: GetInt(fields[attendeesIndex]),
                    Regime: fields[regimeIndex],
                    Reason: fields[reasonIndex]
                );
                result.Add(model);
            }
            return result.ToImmutableArray();
        }
        public static ImmutableDictionary<string, SchoolStatus> CreateSchoolsStatusesSummary(ImmutableArray<SchoolAbsenceDay> absences, ImmutableArray<SchoolRegimeDay> regimes)
        {
            var groupedAbsences = absences.GroupBy(d => d.School, d => d).ToDictionary(g => g.Key, g => g.ToImmutableArray());
            var groupedRegimes = regimes.GroupBy(d => d.School, d => d).ToDictionary(g => g.Key, g => g.ToImmutableArray());

            var result = new Dictionary<string, SchoolStatus>();
            foreach (var ga in groupedAbsences)
            {
                if (!result.TryGetValue(ga.Key, out var status))
                {
                    status = new SchoolStatus(ImmutableArray<SchoolAbsenceDay>.Empty.AddRange(ga.Value), ImmutableArray<SchoolRegimeDay>.Empty);
                    result.Add(ga.Key, status);
                }
                else
                {
                    result[ga.Key] = status with { Absences = status.Absences.AddRange(ga.Value) };
                }
            }
            foreach (var gr in groupedRegimes)
            {
                if (!result.TryGetValue(gr.Key, out var status))
                {
                    status = new SchoolStatus(ImmutableArray<SchoolAbsenceDay>.Empty, ImmutableArray<SchoolRegimeDay>.Empty.AddRange(gr.Value));
                    result.Add(gr.Key, status);
                }
                else
                {
                    result[gr.Key] = status with { Regimes = status.Regimes.AddRange(gr.Value) };
                }
            }
            return result.ToImmutableDictionary();
        }
    }
}
