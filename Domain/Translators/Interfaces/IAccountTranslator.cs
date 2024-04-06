using Domain.Dto.Request;
using Domain.Dto.Response;
using Domain.Models;

namespace Domain.Translators.Interfaces
{
    public interface IAccountTranslator
    {
        Account ToDomain(AccountRequest request);
        AccountResponse ToResponse(Account account);
    }
}