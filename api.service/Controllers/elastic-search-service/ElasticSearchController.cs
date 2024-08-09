using BusinessProvider.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using BusinessProvider.Models;


namespace SampleDotNetCoreApiProject.Controllers.elasticsearchservice
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElasticSearchController : ControllerBase
    {
        private readonly IDataService _dataService;

        public ElasticSearchController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
        {
            var response = await _dataService.GetDataById("", id, cancellationToken);
            if (response.Found)
            {
                return Ok(response.Source);
            }
            else
            {
                return NotFound(); // or throw an exception, depending on your requirements
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] DataRequest dataRequest, CancellationToken cancellationToken)
        {
            if (dataRequest == null)
            {
                return BadRequest("Invalid data request");
            }

            var response = await _dataService.AddOrUpdate(dataRequest, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("createOrUpdateIndex")]
        public async Task<IActionResult> CreateOrUpdateIndex(string indexName)
        {
            var response = await _dataService.CreateOrUpdateIndex(indexName);

            return Ok(response);
        }

        [HttpGet]
        [Route("retrieveMapping")]
        public async Task<IActionResult> RetrieveMapping()
        {
            var response = await _dataService.RetrieveMappingAsync();

            return Ok(response);
        }
    }
}