using Nest;
namespace BusinessProvider.Models.ElasticSearch
{
    public class Account : BaseRecord
    {
        [Keyword(Name = "accountName")]
        public required string AccountName { get; set; }

        [Keyword(Name = "type")]
        public required string Type { get; set; }

        [Keyword(Name = "isActive")]
        public bool IsActive { get; set; } = true;
    }
}