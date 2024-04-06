using DataProvider.Repositories;
using Domain.Dto.Request;
using Domain.Dto.Response;
using Domain.Translators.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SampleDotNetCoreApiProject.Controllers.ExpenseMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController: BaseController
    {
        private readonly IAccountTranslator _accountTranslator;
        
        public AccountController(IAccountTranslator accountTranslator, IUnitOfWork unitOfWork): base(unitOfWork)
        {
            _accountTranslator = accountTranslator;
        }

        // GET: api/products
        // [HttpGet]
        // public ActionResult<IEnumerable<Account>> Get()
        // {
           // return _dbContext.Account.ToList();
       // }

        // GET: api/products/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AccountResponse>> Get(Guid id)
        {
            var account = await _unitOfWork.Account.GetById(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(_accountTranslator.ToResponse(account));
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] AccountRequest account)
        {
            var result = await _unitOfWork.Account.Add(_accountTranslator.ToDomain(account));
            if(result)
            {
                await _unitOfWork.CompleteAsync();
            }
            return Ok(result);
        }

        // PUT: api/products/5
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<bool>> Put(Guid id, [FromBody] AccountRequest account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            var result = await _unitOfWork.Account.Update(_accountTranslator.ToDomain(account));
            if (result)
            {
                await _unitOfWork.CompleteAsync();
            }
            return Ok(result);
        }

        // DELETE: api/products/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _unitOfWork.Account.Delete(id);
            if (result)
            {
                await _unitOfWork.CompleteAsync();
            }

            return Ok(result);
        }
    }
}