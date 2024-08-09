using Nest;
namespace BusinessProvider.Models.ElasticSearch
{
    public class SysData
    {
        [Keyword(Name = "sysTenant")]
        public required string sysTenant { get; set; }

        [Keyword(Name = "sysSubTenant")]
        public string? sysSubTenant { get; set; }

        [Keyword(Name = "sysLocale")]
        public required string sysLocale { get; set; }

        [TextWithKeyword]
        public string? sysDomainType { get; set; }

        [Keyword(Name = "sysCreatedBy")]
        public required string sysCreatedBy { get; set; }

        [Date(Name = "sysCreatedDate")]
        public DateTime sysCreatedDate { get; set; }

        [Keyword(Name = "sysModBy")]
        public required string sysModBy { get; set; }

        [Date(Name = "sysModDate")]
        public DateTime sysModDate { get; set; }
    }
}