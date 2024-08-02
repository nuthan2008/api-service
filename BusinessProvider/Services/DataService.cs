using System;
using BusinessProvider.Domain.Services;
using BusinessProvider.Models.DataSvc;
using Nest;

namespace BusinessProvider.Services
{
    public class DataService : IDataService
    {
        private readonly IElasticSearchService<Practice> _esService;

        public DataService(IElasticSearchService<Practice> esService)
        {
            _esService = esService;
            _esService.Index("dataSvc");
        }

        public async Task<GetResponse<Practice>> GetDataById(string type, string id)
        {
            return await _esService.Get(id);
        }
    }
}