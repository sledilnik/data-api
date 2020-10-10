using SloCovidServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SloCovidServer.Services.Implemented
{
    public class Mapper: Mappers.Mapper
    {
        static readonly ImmutableArray<AgeBucketMeta> ageBuckets;
        static readonly int[] ageBucketRangesNew = new[] { 4, 14, 24, 34, 44, 54, 64, 74, 84 };
        static readonly string[] facilities = { "ukclj", "ukcmb", "ukg", "sbce", "sbnm", "sbms", "sbje", "sbsg", "sbpt" };
        static readonly string[] hospitals = { "bse", "bto", "sbbr", "sbce", "sbiz", "sbje", "sbms", "sbng",
            "sbnm", "sbpt", "sbsg", "sbtr", "ukclj", "ukcmb", "ukg" };

        static Mapper()
        {
            ageBuckets = ImmutableArray<AgeBucketMeta>.Empty;
            int start = 0;
            foreach (int r in ageBucketRangesNew)
            {
                ageBuckets = ageBuckets.Add(new AgeBucketMeta(start, r));
                start = r + 1;
            }
            ageBuckets = ageBuckets.Add(new AgeBucketMeta(start, null));
        }
        public ImmutableArray<StatsDaily> GetStatsFromRaw(string raw)
        {
            ImmutableArray<StatsDaily> result = ImmutableArray<StatsDaily>.Empty;
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            StatsDaily previous = null;
            foreach (string line in lines.Skip(1))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var dailyData = GetDailyStatsFromRaw(header, line, previous?.StatePerTreatment.DeceasedToDate, previous?.StatePerTreatment.OutOfHospitalToDate);
                    result = result.Add(dailyData);
                    previous = dailyData;
                }
            }
            return result;
        }

        public ImmutableArray<RegionsDay> GetRegionsFromRaw(string raw)
        {
            ImmutableArray<RegionsDay> result = ImmutableArray<RegionsDay>.Empty;
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            foreach (string line in lines.Skip(1))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var region = GetDailyRegionFromRaw(header, line);
                    result = result.Add(region);
                }
            }
            return result;
        }

        public ImmutableArray<PatientsDay> GetPatientsFromRaw(string raw)
        {
            ImmutableArray<PatientsDay> result = ImmutableArray<PatientsDay>.Empty;
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            foreach (string line in lines.Skip(1))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var region = GetDailyPatientsFromRaw(header, line);
                    result = result.Add(region);
                }
            }
            return result;
        }

        public ImmutableArray<HospitalsDay> GetHospitalsFromRaw(string raw)
        {
            ImmutableArray<HospitalsDay> result = ImmutableArray<HospitalsDay>.Empty;
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            foreach (string line in lines.Skip(1))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var hospital = GetDailyHospitalsFromRaw(header, line);
                    result = result.Add(hospital);
                }
            }
            return result;
        }

        public ImmutableArray<Hospital> GetHospitalsListFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int idIndex = header["id"];
            int nameIndex = header["name"];
            int urlIndex = header["url"];

            var result = lines.Skip(1).Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => GetHospitalFromRaw(idIndex, nameIndex, urlIndex, l))
                .ToImmutableArray();
            
            return result;
        }

        public ImmutableArray<Municipality> GetMunicipalitiesListFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int idIndex = header["id"];
            int nameIndex = header["name"];
            int populationIndex = header["population"];

            var result = lines.Skip(1).Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => GetMunicipalityFromRaw(idIndex, nameIndex, populationIndex, l))
                .ToImmutableArray();

            return result;
        }

        public ImmutableArray<RetirementHome> GetRetirementHomesListFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int idIndex = header["id"];
            int nameIndex = header["name"];
            int regionIndex = header["region"];
            int typeIndex = header["type"];
            int occupantsIndex = header["occupants"];
            int employeesIndex = header["employees"];
            int urlIndex = header["url"];

            var result = lines.Skip(1).Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => {
                    var fields = ParseLine(l);
                    return new RetirementHome(
                        fields[idIndex],
                        fields[nameIndex],
                        fields[regionIndex],
                        fields[typeIndex],
                        GetInt(fields[occupantsIndex]),
                        GetInt(fields[employeesIndex]),
                        fields[urlIndex]
                    );
                })
                .ToImmutableArray();

            return result;
        }

        public ImmutableArray<RetirementHomesDay> GetRetirementHomesFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            int totalIndex = header["rh.total"];
            int employeeIndex = header["rh.employee.total"];
            int occupantIndex = header["rh.occupant.total"];
            var meta = GetRetirementHomeMetaFromHeader(header);

            return lines.Skip(1).Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => GetRetirementHomesDayFromRaw(dateIndex, totalIndex, employeeIndex, occupantIndex, meta, l)).ToImmutableArray();
        }

        ImmutableArray<RetirementHomeMeta> GetRetirementHomeMetaFromHeader(ImmutableDictionary<string, int> header)
        {
            var meta = ImmutableArray<RetirementHomeMeta>.Empty;
            var groupped = from h in header
                           let parts = h.Key.Split('.')
                           where parts.Length == 4
                           group new { Id = parts[2], Type=parts[3], Index = h.Value } by parts[2] into g
                           select g;
            foreach (var g in groupped)
            {
                int totalIndex = 0;
                int employeeIndex = 0;
                int occupantIndex = 0;
                foreach (var value in g)
                {
                    switch (value.Type)
                    {
                        case "total":
                            totalIndex = value.Index;
                            break;
                        case "employee":
                            employeeIndex = value.Index;
                            break;
                        case "occupant":
                            occupantIndex = value.Index;
                            break;
                    }
                }
                meta = meta.Add(new RetirementHomeMeta(g.Key, totalIndex, employeeIndex, occupantIndex));
            }
            return meta;
        }
        RetirementHomesDay GetRetirementHomesDayFromRaw(int dateIndex, int totalIndex, int employeeIndex, int occupantIndex, 
            ImmutableArray<RetirementHomeMeta> meta, string line)
        {
            var fields = ParseLine(line);
            var date = GetDate(fields[dateIndex]);
            var homes = meta.Select(m => 
                new RetirementHomeDay(m.Id,
                    GetInt(fields[m.TotalIndex]),
                    GetInt(fields[m.EmployeeIndex]),
                    GetInt(fields[m.OccupantIndex])
                )
            )
            .ToImmutableArray();
            
            return new RetirementHomesDay(
                date.Year, 
                date.Month,
                date.Day,
                GetInt(fields[totalIndex]),
                GetInt(fields[employeeIndex]),
                GetInt(fields[occupantIndex]),
                homes
            );
        }

        Municipality GetMunicipalityFromRaw(int idIndex, int nameIndex, int populationIndex, string line)
        {
            var fields = ParseLine(line);
            return new Municipality(fields[idIndex], fields[nameIndex], GetInt(fields[populationIndex]) ?? 0);
        }

        Hospital GetHospitalFromRaw(int idIndex, int nameIndex, int urlIndex, string line)
        {
            var fields = ParseLine(line);
            return new Hospital(fields[idIndex], fields[nameIndex], fields[urlIndex]);
        }

        HospitalsDay GetDailyHospitalsFromRaw(ImmutableDictionary<string, int> header, string line)
        {
            var fields = ParseLine(line);
            var date = GetDate(fields[header["date"]]);
            var perHospital = new Dictionary<string, HospitalDay>(facilities.Length);
            foreach (string hospital in hospitals)
            {
                perHospital.Add(hospital, GetHospitalDay(hospital, header, fields));
            }
            return new HospitalsDay(
                date.Year, date.Month, date.Day, 
                overall: GetHospitalDay(null, header, fields),
                perHospital: perHospital.ToImmutableDictionary()
            );
        }

        HospitalDay GetHospitalDay(string hospital, ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            return new HospitalDay(
                GetHospitalBeds(hospital, header, fields),
                GetHospitalICUs(hospital, header, fields),
                GetHospitalVents(hospital, header, fields),
                GetHospitalCare(hospital, header, fields)
            );
        }

        HospitalBedDay GetHospitalBeds(string hospital, ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            string location = !string.IsNullOrEmpty(hospital) ? $".{hospital}" : "";
            return new HospitalBedDay(
                GetInt($"hospital{location}.bed.total", header, fields),
                GetInt($"hospital{location}.bed.total.max", header, fields),
                GetInt($"hospital{location}.bed.occupied", header, fields),
                GetInt($"hospital{location}.bed.free", header, fields),
                GetInt($"hospital{location}.bed.free.max", header, fields)
            );
        }
        HospitalICUDay GetHospitalICUs(string hospital, ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            string location = !string.IsNullOrEmpty(hospital) ? $".{hospital}" : "";
            return new HospitalICUDay(
                GetInt($"hospital{location}.icu.total", header, fields, isMandatory: false),
                GetInt($"hospital{location}.icu.total.max", header, fields, isMandatory: false),
                GetInt($"hospital{location}.icu.occupied", header, fields, isMandatory: false),
                GetInt($"hospital{location}.icu.free", header , fields, isMandatory: false)
            );
        }
        HospitalVentDay GetHospitalVents(string hospital, ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            string location = !string.IsNullOrEmpty(hospital) ? $".{hospital}" : "";
            return new HospitalVentDay(
                GetInt($"hospital{location}.vent.total", header, fields, isMandatory: false),
                GetInt($"hospital{location}.vent.total.max", header, fields, isMandatory: false),
                GetInt($"hospital{location}.vent.occupied", header, fields, isMandatory: false),
                GetInt($"hospital{location}.vent.free", header, fields, isMandatory: false)
            );
        }
        HospitalCareDay GetHospitalCare(string hospital, ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            string location = !string.IsNullOrEmpty(hospital) ? $".{hospital}" : "";
            return new HospitalCareDay(
                GetInt($"hospital{location}.care.total", header, fields, isMandatory: false),
                GetInt($"hospital{location}.care.total.max", header, fields, isMandatory: false),
                GetInt($"hospital{location}.care.occupied", header, fields, isMandatory: false),
                GetInt($"hospital{location}.care.free", header, fields, isMandatory: false)
            );
        }

        RegionsDay GetDailyRegionFromRaw(ImmutableDictionary<string, int> header, string line)
        {
            var fields = ParseLine(line);
            Dictionary<string, Dictionary<string, int?>> result = new Dictionary<string, Dictionary<string, int?>>(StringComparer.OrdinalIgnoreCase);
            foreach (var headerPair in header)
            {
                string[] headerParts = headerPair.Key.Split('.');
                if (headerParts.Length == 3)
                {
                    if (!result.TryGetValue(headerParts[1], out Dictionary<string, int?> regions))
                    {
                        regions = new Dictionary<string, int?>(StringComparer.OrdinalIgnoreCase);
                        result.Add(headerParts[1], regions);
                    }
                    regions[headerParts[2]] = GetInt(fields[headerPair.Value]);
                }
            }
            var date = GetDate(fields[header["date"]]);
            return new RegionsDay(date.Year, date.Month, date.Day, result);
        }

        internal static int? GetDelta(int? currentToDate, int? previousToDate)
        {
            if (currentToDate.HasValue)
            {
                return currentToDate - (previousToDate ?? 0);
            }
            else
            {
                return null;
            }
        }
        StatsDaily GetDailyStatsFromRaw(ImmutableDictionary<string, int> header, string line, int? previousDecasedToDate, int? previousOutOfHospitalToDate)
        {
            var fields = ParseLine(line);
            int? deceasedToDate = GetInt("state.deceased.todate", header, fields);
            int? deceased = GetDelta(deceasedToDate, previousDecasedToDate);
            int? outOfHospitalToDate = GetInt("state.out_of_hospital.todate", header, fields);
            int? outOfHospital = GetDelta(outOfHospitalToDate, previousOutOfHospitalToDate);
            var cases = new Cases(
                GetInt("cases.confirmed", header, fields),
                GetInt("cases.confirmed.todate", header, fields),
                GetInt("cases.recovered.todate", header, fields),
                GetInt("cases.closed.todate", header, fields),
                GetInt("cases.active", header, fields),
                new HealthSystemSCases(GetInt("cases.hs.employee.confirmed.todate", header, fields)),
                new RetirementHomeCases(
                    GetInt("cases.rh.employee.confirmed.todate", header, fields),
                    GetInt("cases.rh.occupant.confirmed.todate", header, fields)
                ),
                new UnclassifiedCases(GetInt("cases.unclassified.confirmed.todate", header, fields))
            );
            var perTreatment = new PerTreatment(
                GetInt("state.in_hospital", header, fields),
                GetInt("state.in_hospital.todate", header, fields),
                GetInt("state.icu", header, fields),
                GetInt("state.critical", header, fields),
                deceasedToDate,
                deceased,
                outOfHospitalToDate,
                outOfHospital,
                GetInt("state.recovered.todate", header, fields)
            );
            var perRegion = ImmutableDictionary<string, int?>.Empty;
            foreach (var pair in header)
            {
                string[] keyParts = pair.Key.Split('.');
                if (keyParts.Length == 3 && keyParts[0] == "region" && keyParts[2] == "todate")
                {
                    perRegion = perRegion.Add(keyParts[1], GetInt(fields[pair.Value]));
                }
            }
            var perAgeSum = ImmutableArray<PerAgeBucket>.Empty;
            foreach (var bucket in ageBuckets)
            {
                var perAge = new PerAgeBucket(
                    bucket.AgeFrom,
                    bucket.AgeTo,
                    GetInt($"age.{bucket.Key}.todate", header, fields),
                    GetInt($"age.female.{bucket.Key}.todate", header, fields),
                    GetInt($"age.male.{bucket.Key}.todate", header, fields)
                );
                perAgeSum = perAgeSum.Add(perAge);
            }
            var deceasedPerAge = ImmutableArray<PerAgeBucket>.Empty;
            foreach (var bucket in ageBuckets)
            {
                var perAge = new PerAgeBucket(
                    bucket.AgeFrom,
                    bucket.AgeTo,
                    GetInt($"deceased.{bucket.Key}.todate", header, fields),
                    GetInt($"deceased.female.{bucket.Key}.todate", header, fields),
                    GetInt($"deceased.male.{bucket.Key}.todate", header, fields)
                );
                deceasedPerAge = deceasedPerAge.Add(perAge);
            }
            var date = GetDate(fields[header["date"]]);
            var result = new StatsDaily(
                GetInt("day", header, fields) ?? 0,
                date.Year,
                date.Month,
                date.Day,
                fields[header["phase"]],
                GetInt("tests.performed.todate", header, fields),
                GetInt("tests.performed", header, fields),
                GetInt("tests.positive.todate", header, fields),
                GetInt("tests.positive", header, fields),
                new Tests(
                    performed: new CommonTests(
                        GetInt("tests.performed.todate", header, fields),
                        GetInt("tests.performed", header, fields)
                    ),
                    positive: new CommonTests(
                        GetInt("tests.positive.todate", header, fields),
                        GetInt("tests.positive", header, fields)
                    ),
                    regular: new RegularTests(
                        performed: new CommonTests(
                            GetInt("tests.regular.performed.todate", header, fields),
                            GetInt("tests.regular.performed", header, fields)
                        ),
                        positive: new CommonTests(
                            GetInt("tests.regular.positive.todate", header, fields),
                            GetInt("tests.regular.positive", header, fields)
                        )
                    ),
                    nSApr20: new RegularTests(
                        performed: new CommonTests(
                            GetInt("tests.ns-apr20.performed.todate", header, fields),
                            GetInt("tests.ns-apr20.performed", header, fields)
                        ),
                        positive: new CommonTests(
                            GetInt("tests.ns-apr20.positive.todate", header, fields),
                            GetInt("tests.ns-apr20.positive", header, fields)
                        )
                    )
                ),
                GetInt("age.female.todate", header, fields),
                GetInt("age.male.todate", header, fields),
                cases,
                perTreatment,
                perRegion,
                perAgeSum,
                deceasedPerAge
            );
            return result;
        }

        PatientsDay GetDailyPatientsFromRaw(ImmutableDictionary<string, int> header, string line)
        {
            var fields = ParseLine(line);
            Dictionary<string, Dictionary<string, int?>> result = new Dictionary<string, Dictionary<string, int?>>();
            foreach (var headerPair in header)
            {
                string[] headerParts = headerPair.Key.Split('.');
                if (headerParts.Length == 3)
                {
                    if (!result.TryGetValue(headerParts[1], out Dictionary<string, int?> regions))
                    {
                        regions = new Dictionary<string, int?>();
                        result.Add(headerParts[1], regions);
                    }
                    regions[headerParts[2]] = GetInt(fields[headerPair.Value]);
                }
            }
            var date = GetDate(fields[header["date"]]);
            var generalUnit = new GeneralUnit(
                inHospital: GetHospitalMovement(facility: null, "in_hospital", header, fields),
                GetHospitalMovement(facility: null, "icu", header, fields),
                GetHospitalMovement(facility: null, "critical", header, fields),
                GetStateDeceased(header, fields),
                new OutOfHospital(GetInt(fields[header["state.out_of_hospital.todate"]]))
                );
            ImmutableDictionary<string, Unit> f = ImmutableDictionary<string, Unit>.Empty;
            foreach (string facility in facilities)
            {
                var unit = new Unit(
                    inHospital: GetHospitalMovement(facility, "in_hospital", header, fields),
                    GetHospitalMovement(facility, "icu", header, fields),
                    GetHospitalMovement(facility, "critical", header, fields),
                    GetDeceased(facility, header, fields)
                );
                f = f.Add(facility, unit);
            }
            return new PatientsDay(GetInt(fields[header["day"]]) ?? 0, date.Year, date.Month, date.Day, generalUnit, f);
        }

        HospitalMovement GetHospitalMovement(string facility, string type, ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            string location = !string.IsNullOrEmpty(facility) ? $".{facility}" : "";
            return new HospitalMovement(
                GetInt(fields[header[$"state{location}.{type}.in"]]),
                GetInt(fields[header[$"state{location}.{type}.out"]]),
                GetInt(fields[header[$"state{location}.{type}"]]),
                GetInt(fields[header[$"state{location}.{type}.todate"]])
            );
        }

        Movement GetMovement(string facility, string type, ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            string location = !string.IsNullOrEmpty(facility) ? $".{facility}.{type}" : $".{type}";
            return new Movement(
                GetInt(fields[header[$"state{location}.in"]]),
                GetInt(fields[header[$"state{location}.out"]]),
                GetInt(fields[header[$"state{location}"]])
            );
        }

        HospitalDeceased GetDeceased(string facility, ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            string location = $".{facility}";
            return new HospitalDeceased(
                GetInt(fields[header[$"state{location}.deceased"]]),
                GetInt(fields[header[$"state{location}.deceased.todate"]]),
                new ToDateToday(
                    GetInt(fields[header[$"state{location}.deceased.icu"]]),
                    GetInt(fields[header[$"state{location}.deceased.icu.todate"]])
                )
            );
        }

        StateDeceased GetStateDeceased(ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            return new StateDeceased(
                GetInt(fields[header[$"state.deceased"]]),
                GetInt(fields[header[$"state.deceased.todate"]]),
                new StateDeceased.HospitalStats(
                    GetInt(fields[header[$"state.deceased.hospital"]]),
                    GetInt(fields[header[$"state.deceased.hospital.todate"]]),
                    icu: new ToDateToday(
                        GetInt(fields[header[$"state.deceased.hospital.icu"]]),
                        GetInt(fields[header[$"state.deceased.hospital.icu.todate"]])
                        )
                    ),
                home: new ToDateToday(
                        GetInt(fields[header[$"state.deceased.home"]]),
                        GetInt(fields[header[$"state.deceased.home.todate"]])
                )
            );
        }

        public ImmutableArray<ImmutableArray<object>> MapRegionsPivot(ImmutableArray<Municipality> municipalities,
            ImmutableArray<RegionsDay> regions)
        {
            var sloveneCulture = new CultureInfo("sl-SI");
            var result = new List<ImmutableArray<object>>(municipalities.Length);
            {
                // add header row
                // skip first = Slovenija
                var row = new List<object>();
                row.Add("Place");
                row.Add("Population");
                foreach (var r in regions)
                {
                    string dayOfWeek = sloveneCulture.DateTimeFormat.GetAbbreviatedDayName(new DateTime(r.Year, r.Month, r.Day).DayOfWeek);
                    row.Add($"{r.Day}. {r.Month}. {dayOfWeek}");
                }
                result.Add(row.ToImmutableArray());
            }
            foreach (var m in municipalities.Skip(1))
            {
                var row = new List<object>();
                row.Add(m.Name);
                row.Add(m.Population);
                foreach (var r in regions)
                {
                    string dayOfWeek = sloveneCulture.DateTimeFormat.GetAbbreviatedDayName(new DateTime(r.Year, r.Month, r.Day).DayOfWeek);
                    row.Add(r.FindByPlace(m.Id));
                }
                result.Add(row.ToImmutableArray());
            }
            return result.ToImmutableArray();
        }
    }

    [DebuggerDisplay("{Key,nq}")]
    public class AgeBucketMeta
    {
        public string Key { get; }
        public string TargetName { get; }
        public int? AgeFrom { get; }
        public int? AgeTo { get; }
        public AgeBucketMeta(int ageFrom, int? ageTo)
        {
            AgeFrom = ageFrom;
            AgeTo = ageTo;
            if (ageTo.HasValue)
            {
                TargetName = $"from{ageFrom}to{ageTo}";
                Key = $"{AgeFrom}-{AgeTo}";
            }
            else
            {
                TargetName = $"above{ageFrom}";
                Key = $"{AgeFrom}+";
            }
        }
    }

    public readonly struct RetirementHomeMeta
    {
        public string Id { get; }
        public int TotalIndex { get; }
        public int EmployeeIndex { get; }
        public int OccupantIndex { get; }
        public RetirementHomeMeta(string id, int totalIndex, int employeeIndex, int occupantIndex)
        {
            Id = id;
            TotalIndex = totalIndex;
            EmployeeIndex = employeeIndex;
            OccupantIndex = occupantIndex;
        }
    }
}
