using BusinessProvider.Domain.Services;
using BusinessProvider.Models.DataSvc;
using BusinessProvider.providers;
using BusinessProvider.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Serilog;


namespace APIService.Controllers.DataService;

[ApiController]
[Route("api/[controller]")]
public class BusinessController : ControllerBase
{
    private readonly IBusinessLogicProvider _businessProvider;
    private readonly IAbstractFactory<IDataService> _abstractFactory;
    readonly ILogger<BusinessController> _logger;

    private readonly IElasticClient _elasticClient;

    public BusinessController(
        IBusinessLogicProvider businessProvider,
        IAbstractFactory<IDataService> abstractFactory,
        ILogger<BusinessController> logger,
        IElasticClient elasticClient)
    {
        _businessProvider = businessProvider;
        _abstractFactory = abstractFactory;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _elasticClient = elasticClient;
    }

    [HttpPost]
    [Route("format")]
    public async Task<IActionResult> getFormattedData(string id, CancellationToken cancellationToken)
    {

        var response = await _businessProvider.getDataById(id, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    [Route("format1123")]
    public async Task<IActionResult> getFormattedData1(string id, CancellationToken cancellationToken)
    {

        var response = await _businessProvider.getDataById(id, cancellationToken);

        return Ok(response);
    }


    [HttpGet(Name = "GetPractices")]
    [Authorize]
    public async Task<IActionResult> Get(string keyword)
    {
        _logger.LogInformation("Testing");

        var result = await _elasticClient.SearchAsync<Practice>(
            s => s.Query(
                q => q.QueryString(
                    d => d.Query('*' + keyword + '*')
                )
            ).Size(100)
        );
        return Ok(result.Documents.ToList());
    }

    [HttpPost(Name = "AddPractice")]
    public async Task<IActionResult> Post(Practice practice)
    {
        await _elasticClient.IndexDocumentAsync(practice);
        return Ok();
    }
}
