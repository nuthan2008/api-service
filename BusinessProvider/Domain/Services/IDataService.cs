using BusinessProvider.Models.DataSvc;
using Nest;
namespace BusinessProvider.Domain.Services
{
    public interface IDataService
    {
        Task<GetResponse<Practice>> GetDataById(string type, string Id);
    }
}

