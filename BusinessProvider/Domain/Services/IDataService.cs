using System.Text.Json;
using BusinessProvider.Models;
using Nest;
namespace BusinessProvider.Domain.Services
{
    public interface IDataService
    {
        Task<GetResponse<DataRequest>> GetDataById(string type, string Id, CancellationToken cancellationToken);

        Task<DataRequest> AddOrUpdate(DataRequest request, CancellationToken cancellationToken);

        Task<string> CreateOrUpdateIndex(string indexName);

        Task<JsonElement> RetrieveMappingAsync();
    }
}