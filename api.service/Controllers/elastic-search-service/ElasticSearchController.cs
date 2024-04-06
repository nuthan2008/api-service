using System;
using BusinessProvider.Domain.Services;
using BusinessProvider.providers;
using BusinessProvider.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;
using ElasticsearchCRUD;


namespace SampleDotNetCoreApiProject.Controllers.elasticsearchservice
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElasticSearchController : ControllerBase
    {
        // private readonly IElasticSearchIndexProvider _elasticSearchIndexProvider;

        public ElasticSearchController() // IElasticSearchIndexProvider elasticSearchIndexProvider)
        {
            // _elasticSearchIndexProvider = elasticSearchIndexProvider;
        }

        [HttpPost]
        [Route("createIndex")]
        public async Task<IActionResult> createIndex(string indexName, CancellationToken cancellationToken)
        {
            var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .EnableApiVersioningHeader();
            var client = new ElasticClient(connectionSettings);
            var elasticsearch = new ElasticSearch(client, indexName);
            await elasticsearch.CreateIndexIfNotExists(indexName);
            var document = new { id = 1, name = "Name 001" };
            var response = await elasticsearch.AddOrUpdate(document);

            // var response =await _elasticSearchIndexProvider.GetDocument(cancellationToken);

            return Ok(response);
        }
    }
}

