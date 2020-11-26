using System.Text.Json.Serialization;

namespace SloCovidServer.Models
{
    public record MonthlyDeathsSlovenia: IModelDate
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Deceased { get; init; }
        [JsonIgnore]
        public int Day => 1;
    }
}
