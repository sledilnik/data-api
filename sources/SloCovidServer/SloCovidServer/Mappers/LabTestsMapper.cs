using SloCovidServer.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Principal;

namespace SloCovidServer.Mappers
{
    public partial class LabTestsMapper: Mapper
    {
        public ImmutableArray<LabTestDay> MapFromRaw(string raw)
        {
            string[] lines = raw.Split('\n');
            var header = ParseHeader(lines[0]);
            int dateIndex = header["date"];
            var result = new List<LabTestDay>(lines.Length-1);
            {
                foreach (string line in IterateLines(lines))
                {
                    var fields = ParseLine(line);
                    var date = GetDate(fields[dateIndex]);
                    var hospitals = new Dictionary<string, LabTestData>();
                    var labs = new Dictionary<string, LabTestData>();
                    LabTestData total = LabTestData.Empty;
                    foreach (var column in header.Where(p => p.Key != "date"))
                    {
                        int? value = GetInt(fields[column.Value]);
                        switch (column.Key)
                        {
                            case "tests.performed":
                                total = total with { Performed = total.Performed with { Today = value } };
                                break;
                            case "tests.performed.todate":
                                total = total with { Performed = total.Performed with { ToDate = value } };
                                break;
                            case "tests.positive":
                                total = total with { Positive = total.Positive with { Today = value } };
                                break;
                            case "tests.positive.todate":
                                total = total with { Positive = total.Positive with { ToDate = value } };
                                break;
                            default:
                                Dictionary<string, LabTestData> target;
                                int hospitalIndex;
                                if (column.Key.StartsWith ("tests.lab."))
                                {
                                    hospitalIndex = 2;
                                    target = labs;
                                }
                                else
                                {
                                    hospitalIndex = 1;
                                    target = hospitals;
                                }
                                string[] parts = column.Key.Split('.');
                                string hospital = parts[hospitalIndex];
                                if (!target.TryGetValue(hospital, out var labTestData))
                                {
                                    labTestData = LabTestData.Empty;
                                }
                                labTestData = (parts[hospitalIndex+1], parts.Length > hospitalIndex + 2 ? parts[hospitalIndex + 2] : null) switch
                                {
                                    ("performed", null) => labTestData with { Performed = labTestData.Performed with { Today = value } },
                                    ("performed", "todate") => labTestData with { Performed = labTestData.Performed with { ToDate = value } },
                                    ("positive", null) => labTestData with { Positive = labTestData.Positive with { Today = value } },
                                    ("positive", "todate") => labTestData with { Positive = labTestData.Positive with { ToDate = value } },
                                    _ => null,
                                };
                                target[hospital] = labTestData;
                                break;
                        }
                    }
                    result.Add(new LabTestDay(date.Year, date.Month, date.Day, total, hospitals.ToImmutableDictionary(), labs.ToImmutableDictionary()));

                }
            }
            return result.ToImmutableArray();
        }
    }
}
