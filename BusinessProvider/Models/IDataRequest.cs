
namespace BusinessProvider.Models
{
    public interface IDataRequest
    {
        Guid? requestId { get; set; }

        RequestParam? Params { get; set; }

        string Id { get; set; }

        string BusinessName { get; set; }

        string Type { get; set; }

        int IncorporationYear { get; set; }

        SysData SysData { get; set; }
    }
}