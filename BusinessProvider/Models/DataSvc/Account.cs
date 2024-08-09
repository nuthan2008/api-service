namespace BusinessProvider.Models.DataSvc
{
    public class Account
    {
        public string Id { get; set; }
        public required string AccountName { get; set; }
        public required string Type { get; set; }
        public bool IsActive { get; set; } = true;
    }
}