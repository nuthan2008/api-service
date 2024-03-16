
namespace BusinessProvider.Models
{
    public interface IRequestParam
    {
        string Id { get; set; }

        string App { get; set; }

        string Service { get; set; }
        string Types { get; set; }

        string Tenant { get; set; }

        string Subtenant { get; set; }

        string Locale { get; set; }

        string DataLocale { get; set; }

    }
}