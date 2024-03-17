namespace BusinessProvider.Models.DataSvc
{
    public class SoloPractices : ISoloPractice
    {
        public string Id { get; set; }

        public string BusinessName { get; set; }

        public int IncorporationYear { get; set; }

        public bool IsSoloPractice { get; } = true;
    }
}