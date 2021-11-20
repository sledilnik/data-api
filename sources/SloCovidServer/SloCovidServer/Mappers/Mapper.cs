using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Diagnostics;

namespace SloCovidServer.Mappers
{
    public abstract class Mapper
    {
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
        internal IEnumerable<string> IterateLines(string[] lines)
        {
            foreach (string line in lines.Skip(1).Where(l => !string.IsNullOrWhiteSpace(l)))
            {
                yield return line;
            }
        }

        internal int? GetInt(string name, ImmutableDictionary<string, int> header, IImmutableList<string> fields, bool isMandatory = true)
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
        internal int? GetInt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            return ParseInt(text);
        }
        internal float? GetFloat(string text)
        {
            if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
            return null;
        }

        internal Models.Date GetDate(string text)
        {
            string[] parts = text.Split('-');
            // in case date contains TimeZone
            string day = parts[2].Contains('T') ? parts[2].Split('T')[0] : parts[2];
            return new Models.Date(
                year: ParseInt(parts[0]),
                month: ParseInt(parts[1]),
                day: ParseInt(day)
            );
        }

        int ParseInt(string text) => int.Parse(text.Replace(".", ""), CultureInfo.InvariantCulture);

        /// <summary>
        /// Collects all age values that are prefixed by <paramref name="root"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="header"></param>
        /// <param name="fields"></param>
        /// <param name="root"></param>
        /// <param name="parseValue"></param>
        /// <returns></returns>
        /// <example>
        /// Will collect following values
        /// episari.covid.in.age.mean,
        /// episari.covid.in.age.00-04,
        /// episari.covid.in.age.05-14,
        /// ...
        /// when <paramref name="root"/> is episari.covid.in.age
        /// </example>
        internal ImmutableDictionary<string, T> CollectAgeValues<T>(ImmutableDictionary<string, int> header, ImmutableArray<string> fields,
            string root, Func<string, (bool HasValue, T Value)> parseValue)
        {
            const int DefaultCapacity = 12;
            var result = new Dictionary<string, T>(DefaultCapacity);
            foreach (var pair in header
                .Where(v => v.Key.Length > root.Length && v.Key.StartsWith(root, StringComparison.Ordinal)))
            {
                var age = pair.Key.AsSpan()[(root.Length+1)..];
                if (!age.Contains('.'))
                {
                    var (hasValue, value) = parseValue(fields[pair.Value]);
                    if (hasValue)
                    {
                        result.Add(age.ToString(), value);
                    }
                }
            }
            return result.ToImmutableDictionary();
        }

        internal (bool HasValue, int Value) GetIntAgeValue(string text)
        {
            int? value = GetInt(text);
            return (value.HasValue, value ?? default);
        }
    }

    [DebuggerDisplay("{Key,nq}")]
    public record AgeBucketMeta
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
