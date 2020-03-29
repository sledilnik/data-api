namespace SloCovidServer.Models
{
    public class Hospital
    {
        public string Code { get; }
        public string Name { get; }
        public string Uri { get; }
        public Hospital(string code, string name, string uri)
        {
            Code = code;
            Name = name;
            Uri = uri;
        }
    }
}
