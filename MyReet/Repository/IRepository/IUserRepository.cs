using MyReet.Models;
using MyReet.Models.Dto;

namespace MyReet.Repository.IRepostiory
{
	public interface IUserRepository
	{
		bool IsUniqueUser(string username);
		Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
		Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO);
	}
}