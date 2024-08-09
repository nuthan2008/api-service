using System;
using System.Text.Json;
using BusinessProvider.Domain.Services;
using BusinessProvider.Models;
using BusinessProvider.Models.DataSvc;
using Nest;
using AccountModel = BusinessProvider.Models.ElasticSearch.Account;
using SysDataModel = BusinessProvider.Models.ElasticSearch.SysData;

namespace BusinessProvider.Services
{
    public class AccountService : IAccountService
    {
        private readonly IElasticSearchService<AccountModel> _esService;

        public AccountService(IElasticSearchService<AccountModel> esService)
        {
            _esService = esService;
            _esService.Index("expenseappdata");
        }

        public async Task<Account> GetDataById(string type, string id, CancellationToken cancellationToken)
        {
            var response = await _esService.Get(id, cancellationToken);
            if (response.Found)
            {
                return new Account
                {
                    Id = response.Source.Id,
                    AccountName = response.Source.AccountName,
                    Type = response.Source.Type,
                    IsActive = response.Source.IsActive
                };
            }
            else
            {
                throw new Exception("Account not found");
            }
        }

        public async Task<Account> AddOrUpdate(Account request, CancellationToken cancellationToken)
        {

            var accountRequest = new AccountModel
            {
                Id = request.Id,
                AccountName = request.AccountName,
                Type = request.Type,
                IsActive = request.IsActive,
                SysData = new SysDataModel
                {
                    sysTenant = "datasvc",
                    sysLocale = "en_US",
                    sysCreatedBy = "System",
                    sysCreatedDate = DateTime.UtcNow,
                    sysModBy = "System",
                    sysModDate = DateTime.UtcNow
                }
            };


            var response = await _esService.AddOrUpdate(accountRequest, cancellationToken);

            return new Account
            {
                Id = response.Id,
                AccountName = response.AccountName,
                Type = response.Type,
                IsActive = request.IsActive
            };
        }

        public async Task<string> CreateOrUpdateIndex()
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