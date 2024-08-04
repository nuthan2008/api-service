
using Nest;

namespace BusinessProvider.Domain.Services;
public interface IElasticSearchService<T> where T : class
{
    IElasticSearchService<T> Index(string indexName);

    Task<string> EnsureIndexExistsAsync(string indexName);

    Task<BulkResponse> AddOrUpdateBulk(IEnumerable<T> documents);
    Task<T> AddOrUpdate(T document, CancellationToken cancellationToken);
    Task<BulkResponse> AddBulk(IList<T> documents);
    Task<GetResponse<T>> Get(string key, CancellationToken cancellationToken);
    Task<ISearchResponse<T>?> Query(SearchDescriptor<T> sd);
    Task<bool> Remove(string key);
    Task<DeleteByQueryResponse> BulkRemove(IDeleteByQueryRequest<T> queryReq);
}