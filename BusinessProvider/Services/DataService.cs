using System.Text.Json;
using BusinessProvider.Domain.Services;
using BusinessProvider.Models;
using Nest;

namespace BusinessProvider.Services
{
    public class DataService : IDataService
    {
        private readonly IElasticSearchService<DataRequest> _esService;

        public DataService(IElasticSearchService<DataRequest> esService)
        {
            _esService = esService;
            _esService.Index("expenseappdata");
        }

        public async Task<GetResponse<DataRequest>> GetDataById(string type, string id, CancellationToken cancellationToken)
        {
            return await _esService.Get(id, cancellationToken);
        }

        public async Task<DataRequest> AddOrUpdate(DataRequest request, CancellationToken cancellationToken)
        {

            request.SysData = request.SysData ?? new SysData
            {
                sysTenant = "datasvc",
                sysLocale = "en_US",
                sysCreatedBy = "System",
                sysCreatedDate = DateTime.UtcNow,
                sysModBy = "System",
                sysModDate = DateTime.UtcNow
            };
            return await _esService.AddOrUpdate(request, cancellationToken);
        }

        public async Task<string> CreateOrUpdateIndex(string indexName)
        {
            return await _esService.CreateOrUpdateIndex();
        }

        public async Task<JsonElement> RetrieveMappingAsync()
        {
            var mappings = await _esService.RetrieveMappingAsync();

            return mappings;
        }
    }
}