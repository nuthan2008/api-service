using BusinessProvider.Domain.Services;
using BusinessProvider.Models.DataSvc;
using BusinessProvider.providers;
using BusinessProvider.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers.DataService
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessLogicProvider _businessProvider;
        private readonly IAbstractFactory<IDataService> _abstractFactory;

        public BusinessController(IBusinessLogicProvider businessProvider, IAbstractFactory<IDataService> abstractFactory)
        {
            _businessProvider = businessProvider;
            _abstractFactory = abstractFactory;
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


        [HttpGet]
        public async Task<IActionResult> getDataRequest()
        {
            return Ok("test data");
        }
    }
}