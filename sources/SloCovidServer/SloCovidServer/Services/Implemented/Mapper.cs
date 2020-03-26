﻿using SloCovidServer.Models;
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
        static readonly int[] ageBucketRangesLegacy = new[] { 15, 29, 49, 59 };
        static readonly string[] facilities = { "ukclj", "ukcmb", "ukg", "sbce" };

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
            // legacy
            start = 0;
            foreach (int r in ageBucketRangesLegacy)
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
            string[] headerFields = lines[0].Trim().Split(',');
            ImmutableDictionary<string, int> header = ImmutableDictionary<string, int>.Empty;
            for (int i = 0; i < headerFields.Length; i++)
            {
                header = header.Add(headerFields[i], i);
            }
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
            string[] headerFields = lines[0].Trim().Split(',');
            ImmutableDictionary<string, int> header = ImmutableDictionary<string, int>.Empty;
            for (int i = 0; i < headerFields.Length; i++)
            {
                header = header.Add(headerFields[i], i);
            }
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
            string[] headerFields = lines[0].Trim().Split(',');
            ImmutableDictionary<string, int> header = ImmutableDictionary<string, int>.Empty;
            for (int i = 0; i < headerFields.Length; i++)
            {
                header = header.Add(headerFields[i], i);
            }
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

        RegionsDay GetDailyRegionFromRaw(ImmutableDictionary<string, int> header, string line)
        {
            string[] fields = line.Trim().Split(',');
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
            string[] fields = line.Trim().Split(',');
            int? deceasedToDate = GetInt("state.deceased.todate", header, fields);
            int? deceased = GetDelta(deceasedToDate, previousDecasedToDate);
            int? outOfHospitalToDate = GetInt("state.out_of_hospital.todate", header, fields);
            int? outOfHospital = GetDelta(outOfHospitalToDate, previousOutOfHospitalToDate);
            var perTreatment = new PerTreatment(
                GetInt("state.in_hospital", header, fields),
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
            var sourceSum = ImmutableDictionary<string, int?>.Empty;
            foreach (var pair in header)
            {
                string[] keyParts = pair.Key.Split('.');
                if (keyParts.Length == 3 && keyParts[0] == "source" && keyParts[2] == "todate")
                {
                    sourceSum = sourceSum.Add(keyParts[1], GetInt(fields[pair.Value]));
                }
            }
            var facilitySum = ImmutableDictionary<string, int?>.Empty;
            foreach (var pair in header)
            {
                string[] keyParts = pair.Key.Split('.');
                if (keyParts.Length == 3 && keyParts[0] == "facility" && keyParts[2] == "todate")
                {
                    facilitySum = facilitySum.Add(keyParts[1], GetInt(fields[pair.Value]));
                }
            }
            var testsAt14 = new TestsAt14(
                GetInt("tests.performed.14h.todate", header, fields),
                GetInt("tests.performed.14h", header, fields),
                GetInt("tests.positive.14h.todate", header, fields),
                GetInt("tests.positive.14h", header, fields)
                );
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
                testsAt14,
                perTreatment,
                perRegion,
                perAgeSum,
                sourceSum,
                facilitySum
            );
            return result;
        }

        PatientsDay GetDailyPatientsFromRaw(ImmutableDictionary<string, int> header, string line)
        {
            string[] fields = line.Trim().Split(',');
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
                GetInt(fields[header["state.in_care"]]),
                inHospital: GetHospitalMovement(facility: null, header, fields),
                new NeedsO2(GetInt(fields[header["state.needs_o2"]])),
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
                    new NeedsO2(GetInt(fields[header[$"state.{facility}.needs_o2"]])),
                    GetMovement(facility, type: "icu", header, fields),
                    GetMovement(facility, type: "critical", header, fields),
                    GetDeceased(facility, header, fields)
                );
                f = f.Add(facility, unit);
            }
            return new PatientsDay(GetInt(fields[header["day"]]) ?? 0, date.Year, date.Month, date.Day, generalUnit, f);
        }

        HospitalMovement GetHospitalMovement(string facility, ImmutableDictionary<string, int> header, string[] fields)
        {
            string location = !string.IsNullOrEmpty(facility) ? $".{facility}" : "";
            return new HospitalMovement(
                GetInt(fields[header[$"state{location}.in_hospital.in"]]),
                GetInt(fields[header[$"state{location}.in_hospital.out"]]),
                GetInt(fields[header[$"state{location}.in_hospital"]]),
                GetInt(fields[header[$"state{location}.in_hospital.todate"]])
            );
        }

        Movement GetMovement(string facility, string type, ImmutableDictionary<string, int> header, string[] fields)
        {
            string location = !string.IsNullOrEmpty(facility) ? $".{facility}.{type}" : $".{type}";
            return new Movement(
                GetInt(fields[header[$"state{location}.in"]]),
                GetInt(fields[header[$"state{location}.out"]]),
                GetInt(fields[header[$"state{location}"]])
            );
        }

        Deceased GetDeceased(string facility, ImmutableDictionary<string, int> header, string[] fields)
        {
            string location = $".{facility}";
            return new Deceased(
                GetInt(fields[header[$"state{location}.deceased"]])
            );
        }

        StateDeceased GetStateDeceased(ImmutableDictionary<string, int> header, string[] fields)
        {
            return new StateDeceased(
                GetInt(fields[header[$"state.deceased"]]),
                GetInt(fields[header[$"state.deceased.todate"]]),
                GetInt(fields[header[$"state.deceased.hospital"]]),
                GetInt(fields[header[$"state.deceased.home"]])
            );
        }

        int? GetInt(string name, ImmutableDictionary<string, int> header, string[] fields)
        {
            if (!header.TryGetValue(name, out int index))
            {
                throw new Exception($"Can't find field {name}.");
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
