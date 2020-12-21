using SloCovidServer.DB.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Mappers
{
    public class ModelsMapper: Mapper
    {
        public async Task<ImmutableArray<ModelsPredictiondatum>> MapFromRawAsync(Stream csv, CancellationToken ct = default)
        {
            var result = new List<ModelsPredictiondatum>();
            using (StreamReader reader = new StreamReader(csv))
            {
                string headerLine;
                if ((headerLine = await reader.ReadLineAsync()) != null)
                {
                    var header = ParseHeader(headerLine);
                    string lineText;
                    int dateIndex = header[CamelCase(nameof(ModelsPredictiondatum.Date))];
                    int icuIndex = header[CamelCase(nameof(ModelsPredictiondatum.Icu))];
                    int icuLowerBoundIndex = header[CamelCase(nameof(ModelsPredictiondatum.IcuLowerBound))];
                    int icuUpperBoundIndex = header[CamelCase(nameof(ModelsPredictiondatum.IcuUpperBound))];
                    int hospitalizedIndex = header[CamelCase(nameof(ModelsPredictiondatum.Hospitalized))];
                    int hospitalizedLowerBoundIndex = header[CamelCase(nameof(ModelsPredictiondatum.HospitalizedLowerBound))];
                    int hospitalizedUpperBoundIndex = header[CamelCase(nameof(ModelsPredictiondatum.HospitalizedUpperBound))];
                    int deceasedIndex = header[CamelCase(nameof(ModelsPredictiondatum.Deceased))];
                    int deceasedLowerBoundIndex = header[CamelCase(nameof(ModelsPredictiondatum.DeceasedLowerBound))];
                    int deceasedUpperBoundIndex = header[CamelCase(nameof(ModelsPredictiondatum.DeceasedUpperBound))];
                    int deceasedToDateIndex = header[CamelCase(nameof(ModelsPredictiondatum.DeceasedToDate))];
                    int deceasedToDateLowerBoundIndex = header[CamelCase(nameof(ModelsPredictiondatum.DeceasedToDateLowerBound))];
                    int deceasedToDateUpperBoundIndex = header[CamelCase(nameof(ModelsPredictiondatum.DeceasedToDateUpperBound))];
                    while ((lineText = await reader.ReadLineAsync()) != null)
                    {
                        var fields = ParseLine(lineText);
                        var item = new ModelsPredictiondatum
                        {
                            Date = AsUtcDateTime(GetDate(fields[dateIndex])),
                            Icu = GetInt(fields[icuIndex]).Value,
                            IcuLowerBound = GetInt(fields[icuLowerBoundIndex]),
                            IcuUpperBound = GetInt(fields[icuUpperBoundIndex]),
                            Hospitalized = GetInt(fields[hospitalizedIndex]).Value,
                            HospitalizedLowerBound = GetInt(fields[hospitalizedLowerBoundIndex]),
                            HospitalizedUpperBound = GetInt(fields[hospitalizedUpperBoundIndex]),
                            Deceased = GetInt(fields[deceasedIndex]).Value,
                            DeceasedLowerBound = GetInt(fields[deceasedLowerBoundIndex]),
                            DeceasedUpperBound = GetInt(fields[deceasedUpperBoundIndex]),
                            DeceasedToDate = GetInt(fields[deceasedToDateIndex]).Value,
                            DeceasedToDateLowerBound = GetInt(fields[deceasedToDateLowerBoundIndex]),
                            DeceasedToDateUpperBound = GetInt(fields[deceasedToDateUpperBoundIndex]),
                        };
                        result.Add(item);
                    }
                }
            }
            return result.ToImmutableArray();
        }
    }
}
