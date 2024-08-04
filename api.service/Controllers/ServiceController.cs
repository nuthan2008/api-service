using BusinessProvider.Models.DataSvc;
using BusinessProvider.Domain.Services;
using BusinessProvider.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IAbstractFactory<IDataService> _abstractFactory;

        public ServiceController(IAbstractFactory<IDataService> abstractFactory)
        {
            _abstractFactory = abstractFactory;
        }

        [HttpGet("{type}/GetById")]
        public async Task<IActionResult> Get(string type, string id, CancellationToken cancellationToken)
        {
            var serviceInstance = _abstractFactory.Create();
            if (serviceInstance == null)
            {
                return NotFound();
            }



            var result = await serviceInstance.GetDataById(type, id, cancellationToken);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> getDataRequest()
        {
            return Ok("test data from Service Controller");
        }
    }
}