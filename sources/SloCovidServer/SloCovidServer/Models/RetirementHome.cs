namespace SloCovidServer.Models
{
    public class RetirementHome
    {
        public string Id { get; }
        public string Name { get; }
        public string Region { get; }
        public string Type { get; }
        public int? Occupants { get; }
        public int? Employees { get; }
        public string Url { get; }
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
