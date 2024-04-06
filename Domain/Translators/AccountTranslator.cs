using AutoMapper;
using Domain.Dto.Request;
using Domain.Dto.Response;
using Domain.Models;
using Domain.Translators.Interfaces;

namespace Domain.Translators
{
    public class AccountTranslator : IAccountTranslator
    {
        private readonly IMapper _mapper;
        public AccountTranslator(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Account ToDomain(AccountRequest request)
        {
            return _mapper.Map<Account>(request);
        }

        public AccountResponse ToResponse(Account account)
        {
            return _mapper.Map<AccountResponse>(account);
        }
    }
}

