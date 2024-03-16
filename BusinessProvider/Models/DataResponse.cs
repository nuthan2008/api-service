
namespace BusinessProvider.Models
{
    public class DataResponse<T> : IDataResponse<T>
    {
        public string Status { get; set; }
        public string StatusDetails { get; set; }

        public List<T> DataObjs { get; set; }
    }
}