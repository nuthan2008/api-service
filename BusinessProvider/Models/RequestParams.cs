
namespace BusinessProvider.Models
{
    public class RequestParam : IRequestParam
    {
        public string Id { get; set; }

        public string App { get; set; }

        public string Service { get; set; }
        public string Types { get; set; }

        public string Tenant { get; set; }

        public string Subtenant { get; set; }

        public string Locale { get; set; }

        public string DataLocale { get; set; }

    }
}