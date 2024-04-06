using System;
using DataProvider.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace SampleDotNetCoreApiProject.Controllers.ExpenseMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController:ControllerBase
	{
		protected readonly IUnitOfWork _unitOfWork;
		public BaseController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
	}
}

