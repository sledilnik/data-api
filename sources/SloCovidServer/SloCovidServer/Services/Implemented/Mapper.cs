using SloCovidServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SloCovidServer.Services.Implemented
{
    public class Mapper
    {
        static readonly ImmutableArray<AgeBucketMeta> ageBuckets;
        static readonly int[] ageBucketRangesNew = new[] { 4, 14, 24, 34, 44, 54, 64, 74, 84 };
        static readonly string[] facilities = { "ukclj", "ukcmb", "ukg", "sbce" };
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
        internal ImmutableArray<string> ParseLine(string raw)
        {
            string line = raw.Trim();
            var header = ImmutableArray<string>.Empty;
            int index = 0;
            int start = 0;
            bool isInString = false;
            bool wasInString = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (isInString)
                {
                    if (c == '"')
                    {
                        isInString = false;
                    }
                }
                else
                {
                    switch (line[i])
                    {
                        case ',':
                            if (wasInString)
                            {
                                header = header.Add(line.Substring(start + 1, i - start - 2));
                            }
                            else
                            {
                                header = header.Add(line.Substring(start, i - start));
                            }
                            start = i + 1;
                            index++;
                            wasInString = false;
                            break;
                        case '"':
                            isInString = true;
                            wasInString = true;
                            break;
                    }
                }
            }
            if (wasInString)
            {
                header = header.Add(line.Substring(start + 1, line.Length - start - 2));
            }
            else
            {
                header = header.Add(line.Substring(start, line.Length - start));
            }
            return header;
        }
        internal ImmutableDictionary<string, int> ParseHeader(string rawLine)
        {
            var fields = ParseLine(rawLine);
            var header = ImmutableDictionary<string, int>.Empty;
            for (int i = 0; i < fields.Length; i++)
            {
                header = header.Add(fields[i], i);
            }
            return header;
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
                GetHospitalVents(hospital, header, fields)
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
                GetInt("cases.closed.todate", header, fields),
                GetInt("cases.active.todate", header, fields),
                new HealthSystemSCases(GetInt("cases.hs.employee.confirmed.todate", header, fields)),
                new RetirementHomeCases(
                    GetInt("cases.rh.employee.confirmed.todate", header, fields),
                    GetInt("cases.rh.occupant.confirmed.todate", header, fields)
                )
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
                GetInt("age.female.todate", header, fields),
                GetInt("age.male.todate", header, fields),
                cases,
                perTreatment,
                perRegion,
                perAgeSum
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
                inHospital: GetHospitalMovement(facility: null, header, fields),
                GetMovement(facility: null, type: "icu", header, fields),
                GetMovement(facility: null, type: "critical", header, fields),
                GetStateDeceased(header, fields),
                new OutOfHospital(GetInt(fields[header["state.out_of_hospital.todate"]]))
                );
            ImmutableDictionary<string, Unit> f = ImmutableDictionary<string, Unit>.Empty;
            foreach (string facility in facilities)
            {
                var unit = new Unit(
                    inHospital: GetHospitalMovement(facility, header, fields),
                    GetMovement(facility, type: "icu", header, fields),
                    GetMovement(facility, type: "critical", header, fields),
                    GetDeceased(facility, header, fields)
                );
                f = f.Add(facility, unit);
            }
            return new PatientsDay(GetInt(fields[header["day"]]) ?? 0, date.Year, date.Month, date.Day, generalUnit, f);
        }

        HospitalMovement GetHospitalMovement(string facility, ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            string location = !string.IsNullOrEmpty(facility) ? $".{facility}" : "";
            return new HospitalMovement(
                GetInt(fields[header[$"state{location}.in_hospital.in"]]),
                GetInt(fields[header[$"state{location}.in_hospital.out"]]),
                GetInt(fields[header[$"state{location}.in_hospital"]]),
                GetInt(fields[header[$"state{location}.in_hospital.todate"]])
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

        Deceased GetDeceased(string facility, ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            string location = $".{facility}";
            return new Deceased(
                GetInt(fields[header[$"state{location}.deceased"]])
            );
        }

        StateDeceased GetStateDeceased(ImmutableDictionary<string, int> header, IImmutableList<string> fields)
        {
            return new StateDeceased(
                GetInt(fields[header[$"state.deceased"]]),
                GetInt(fields[header[$"state.deceased.todate"]]),
                GetInt(fields[header[$"state.deceased.hospital"]]),
                GetInt(fields[header[$"state.deceased.home"]])
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

        int? GetInt(string name, ImmutableDictionary<string, int> header, IImmutableList<string> fields, bool isMandatory = true)
        {
            if (!header.TryGetValue(name, out int index))
            {
                if (isMandatory)
                {
                    throw new Exception($"Can't find field {name}.");
                }
                else
                {
                    return null;
                }
            }
            string text = fields[index];
            return GetInt(text);
        }
        int? GetInt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            return ParseInt(text);
        }

        (int Year, int Month, int Day) GetDate(string text)
        {
            string[] parts = text.Split('-');
            return (
                Year: ParseInt(parts[0]),
                Month: ParseInt(parts[1]),
                Day: ParseInt(parts[2])
            );
        }

        int ParseInt(string text) => int.Parse(text.Replace(".", ""), CultureInfo.InvariantCulture);
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
}
