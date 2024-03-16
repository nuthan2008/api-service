namespace BusinessProvider.Models.DataSvc
{
    public interface IPractice
    {
        string Id { get; set; }
        string BusinessName { get; set; }
        int IncorporationYear { get; set; }
    }
}