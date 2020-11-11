namespace SloCovidServer.Models
{
    public record Hospital
    {
        public string Code { get; init; }
        public string Name { get; init; }
        public string Uri { get; init; }
        public Hospital(string code, string name, string uri)
        {
            Code = code;
            Name = name;
            Uri = uri;
        }
    }
}
