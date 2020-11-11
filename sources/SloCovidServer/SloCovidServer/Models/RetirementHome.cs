namespace SloCovidServer.Models
{
    public record RetirementHome
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Region { get; init; }
        public string Type { get; init; }
        public int? Occupants { get; init; }
        public int? Employees { get; init; }
        public string Url { get; init; }
        public RetirementHome(string id, string name, string region, string type, int? occupants, int? employees, string url)
        {
            Id = id;
            Name = name;
            Region = region;
            Type = type;
            Occupants = occupants;
            Employees = employees;
            Url = url;
        }
    }
}
