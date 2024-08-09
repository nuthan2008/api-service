
using Nest;

namespace BusinessProvider.Models
{
    public class DataRequest //: IDataRequest
    {
        // public Guid? requestId { get; set; }

        // public RequestParam? Params { get; set; }

        [Keyword(Name = "id")]
        public string Id { get; set; }

        [Text(Name = "name")]
        public string Name { get; set; }

        [Keyword(Name = "type")]
        public string Type { get; set; }

        [Nested(Name = "sysData")]
        public SysData? SysData { get; set; }
    }
}