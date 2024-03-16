
namespace BusinessProvider.Models
{
    public class ApiResponse<T> : IApiResponse<T>
    {
        public DataResponse<T> response { get; set; }
    }
}