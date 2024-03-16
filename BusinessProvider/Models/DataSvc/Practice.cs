namespace BusinessProvider.Models.DataSvc
{
    public class Practice : IPractice
    {
        public string Id { get; set; }
        public string BusinessName { get; set; }
        public int IncorporationYear { get; set; }
    }
}