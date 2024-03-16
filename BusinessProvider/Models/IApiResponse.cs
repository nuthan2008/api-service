
namespace BusinessProvider.Models
{
    public interface IApiResponse<T>
    {
        DataResponse<T> response { get; set; }
    }
}