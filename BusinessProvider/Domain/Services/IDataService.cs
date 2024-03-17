using System;
namespace BusinessProvider.Domain.Services
{
	public interface IDataService
    {
        Task<object> GetDataById(string type, string Id);
    }
}

