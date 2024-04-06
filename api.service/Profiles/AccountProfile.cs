using AutoMapper;
using Domain.Dto.Request;
using Domain.Dto.Response;
using Domain.Models;

namespace SampleDotNetCoreApiProject.Translator
{
	public class AccountProfile: Profile
	{
		public AccountProfile()
		{
            CreateMap<AccountRequest, Account>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1)
            );

            CreateMap<Account, AccountResponse>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name)
            );
        }
	}
}

