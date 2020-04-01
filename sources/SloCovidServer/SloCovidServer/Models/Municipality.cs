namespace SloCovidServer.Models
{
    public class Municipality
    {
        public string Id { get; }
        public string Name { get; }
        public int Population { get; }
        public Municipality(string id, string name, int population)
        {
            Id = id;
            Name = name;
            Population = population;
        }
    }
}
