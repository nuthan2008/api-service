using System.Text.Json;
using BusinessProvider.Models.DataSvc;

namespace BusinessProvider.Domain.Services
{
    public interface IAccountService
    {
        Task<Account> GetDataById(string type, string Id, CancellationToken cancellationToken);

        Task<Account> AddOrUpdate(Account request, CancellationToken cancellationToken);

        Task<string> CreateOrUpdateIndex();

        Task<JsonElement> RetrieveMappingAsync();
    }
}