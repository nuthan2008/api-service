using Nest;
namespace BusinessProvider.Models.ElasticSearch
{
    public class BaseRecord
    {
        [Keyword(Name = "id")]
        public string Id { get; set; }

        [Nested(Name = "sysData")]
        public required SysData SysData { get; set; }
    }
}