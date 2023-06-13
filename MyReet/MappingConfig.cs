using AutoMapper;
using MyReet.Models;
using MyReet.Models.Dto;


namespace MyReet
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
			CreateMap<ApplicationUser, UserDTO>().ReverseMap();

		}
	}
}
