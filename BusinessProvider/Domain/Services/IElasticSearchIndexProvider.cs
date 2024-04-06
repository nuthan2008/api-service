using System;
namespace BusinessProvider.Domain.Services
{
	public interface IElasticSearchIndexProvider
    {
        Task<string> CreateIndex(string indexName, CancellationToken cancellationToken);

        Task<string> GetDocument(CancellationToken cancellationToken);
    }
}