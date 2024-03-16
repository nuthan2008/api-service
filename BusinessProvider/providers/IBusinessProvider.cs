using BusinessProvider.Models;
using BusinessProvider.Models.DataSvc;

namespace BusinessProvider.providers
{
    public interface IBusinessLogicProvider
    {
        Task<IApiResponse<Practice>> getDataById(string Id, CancellationToken cancellationToken);
    }
}