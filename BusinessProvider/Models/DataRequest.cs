
namespace BusinessProvider.Models
{
    public class DataRequest : IDataRequest
    {
        public Guid? requestId { get; set; }

        public RequestParam? Params { get; set; }

        public string Id { get; set; }

        public string BusinessName { get; set; }

        public string Type { get; set; }

        public int IncorporationYear { get; set; }

        public SysData SysData { get; set; }

    }
}