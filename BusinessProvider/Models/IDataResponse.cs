
namespace BusinessProvider.Models
{
    public interface IDataResponse<T>
    {
        string Status { get; set; }
        string StatusDetails { get; set; }

        List<T> DataObjs { get; set; }
    }
}