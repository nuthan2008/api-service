using System.Text.Json;
using BusinessProvider.Domain.Services;
using BusinessProvider.Models.ElasticSearch;
using Nest;
using Elasticsearch.Net;
using BusinessProvider.Utility;

namespace BusinessProvider.Services;

public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
{
    private string IndexName { get; set; }
    private readonly IElasticClient _client;

    public ElasticSearchService(IElasticClient client)
    {
        _client = client;
        IndexName = typeof(T).Name.ToLower() + "s";
    }

    public IElasticSearchService<T> Index(string indexName)
    {
        IndexName = indexName;
        return this;
    }

    public async Task<BulkResponse> AddOrUpdateBulk(IEnumerable<T> documents)
    {
        var indexResponse = await _client.BulkAsync(b => b
            .Index(IndexName)
            .UpdateMany(documents, (ud, d) => ud.Doc(d).DocAsUpsert())
        );
        return indexResponse;
    }

    public async Task<T> AddOrUpdate(T document, CancellationToken cancellationToken)
    {
        var indexResponse =
            await _client.IndexAsync(document, idx => idx.Index(IndexName), cancellationToken);
        if (!indexResponse.IsValid)
        {
            throw new Exception(indexResponse.DebugInformation);
        }

        return document;
    }

    public async Task<BulkResponse> AddBulk(IList<T> documents)
    {
        var resp = await _client.BulkAsync(b => b
            .Index(IndexName)
            .IndexMany(documents)
        );
        return resp;
    }

    public async Task<GetResponse<T>> Get(string key, CancellationToken cancellationToken)
    {
        return await _client.GetAsync<T>(key, g => g.Index("datasvc"), cancellationToken);
    }

    public async Task<List<T>?> GetAll()
    {
        var searchResponse = await _client.SearchAsync<T>(s => s.Index(IndexName).Query(q => q.MatchAll()));
        return searchResponse.IsValid ? searchResponse.Documents.ToList() : default;
    }

    public async Task<ISearchResponse<T>?> Query(SearchDescriptor<T> sd)
    {
        var searchResponse = await _client.SearchAsync<T>(sd);
        return searchResponse;
    }

    public async Task<bool> Remove(string key)
    {
        var response = await _client.DeleteAsync<T>(key, d => d.Index(IndexName));
        return response.IsValid;
    }

    public async Task<DeleteByQueryResponse> BulkRemove(IDeleteByQueryRequest<T> queryReq)
    {
        var response = await _client.DeleteByQueryAsync(queryReq);
        return response;
    }

    public async Task<string> CreateOrUpdateIndex()
    {
        var folderPath = "./mapping/"; // Specify your folder path
        var fileService = new FileService();
        var mappingFiles = await fileService.ReadAllJsonFilesAsync(folderPath);

        foreach (var json in mappingFiles)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var mappingFile = JsonSerializer.Deserialize<MappingFile>(json, options);

            if (mappingFile == null || string.IsNullOrEmpty(mappingFile.IndexName))
            {
                return "Invalid mapping file.";
            }

            var indexName = mappingFile.IndexName;

            var indexExists = _client.Indices.Exists(indexName).Exists;

            if (indexExists)
            {
                var mappingResponse = await UpdateMappingAsync(mappingFile.Mapping, indexName);
                return mappingResponse;
            }
            else
            {
                var response = _client.Indices.Create(indexName, c => c
                    .InitializeUsing(new IndexState())
                    .Map(m => m.AutoMap()));

                if (response.IsValid)
                {
                    var mappingResponse = await UpdateMappingAsync(mappingFile.Mapping, indexName);
                    return mappingResponse;
                }
                else
                {
                    return $"Failed to create index: {response.ServerError}";
                }
            }
        }

        return "No mapping files found.";
    }

    public async Task<JsonElement> RetrieveMappingAsync()
    {
        // Use the low-level client to get the raw JSON mapping
        var mappingResponse = await _client.LowLevel.Indices.GetMappingAsync<StringResponse>(IndexName);

        if (mappingResponse.Success)
        {
            // Parse the raw JSON string into a JsonElement to preserve the structure
            var rawMappingJson = mappingResponse.Body;
            return JsonSerializer.Deserialize<JsonElement>(rawMappingJson);

            //return fieldMappings;
        }
        else
        {
            throw new Exception($"Failed to retrieve mapping for index '{IndexName}': {mappingResponse.Body}");
        }
    }

    private async Task<string> UpdateMappingAsync(object fieldMapping, string indexName)
    {
        // Extract the actual mapping part from the "mappings" key
        var mapping = ((JsonElement)fieldMapping).GetProperty("mappings").GetRawText();

        var response = await _client.LowLevel.Indices.PutMappingAsync<StringResponse>(indexName, mapping);

        if (response.Success)
        {
            return $"Updated mapping for index: {indexName}";
        }
        else
        {
            return $"Failed to update mapping: {response.Body}";
        }
    }
}