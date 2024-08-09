namespace BusinessProvider.Models
{
    public interface ISysData
    {
        string sysTenant { get; set; }

        string sysSubTenant { get; set; }

        string sysLocale { get; set; }

        string sysDomainType { get; set; }

        string sysCreatedBy { get; set; }

        DateTime sysCreatedDate { get; set; }

        string sysModBy { get; set; }

        DateTime sysModDate { get; set; }
    }
}