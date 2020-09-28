using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

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

        internal Models.Date GetDate(string text)
        {
            string[] parts = text.Split('-');
            return new Models.Date(
                year: ParseInt(parts[0]),
                month: ParseInt(parts[1]),
                day: ParseInt(parts[2])
            );
        }

        int ParseInt(string text) => int.Parse(text.Replace(".", ""), CultureInfo.InvariantCulture);
    }
}
