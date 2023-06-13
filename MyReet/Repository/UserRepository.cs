using AutoMapper;
using MyReet.Data;
using MyReet.Models;
using MyReet.Models.Dto;
using MyReet.Repository.IRepostiory;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyReet.Repository
{
	public class UserRepository : IUserRepository
	{

		private readonly ApplicationDbContext _db;
		//private readonly UserManager<ApplicationUser> _userManager;
		//private readonly RoleManager<IdentityRole> _roleManager;
		private string secretKey;
		//private readonly IMapper _mapper;

		public UserRepository(ApplicationDbContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
		{
			_db = db;
			//_mapper = mapper;
			//_userManager = userManager;
			secretKey = configuration.GetValue<string>("ApiSettings:Secret");
			//_roleManager = roleManager;
		}

		public bool IsUniqueUser(string username)
		{
			var user = _db.LocalUsers.FirstOrDefault(x => x.UserName == username);
			if (user == null)
			{
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			var user = _db.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower() && u.Password == loginRequestDTO.Password);

			//bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);


			if (user == null)
			{
				return new LoginResponseDTO()
				{
					Token = "",
					User = null

				};

			}


			//if user was found generate JWT Token
			//var roles = await _userManager.GetRolesAsync(user);
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					  new Claim(ClaimTypes.Name, user.UserName.ToString()),
					  new Claim(ClaimTypes.Role, user.Role)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
			{
				
				Token = tokenHandler.WriteToken(token),
				User = user

			};
			return loginResponseDTO;
		}

		public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
		{
			LocalUser user = new()
			{
				UserName = registerationRequestDTO.UserName,
				FirstName = registerationRequestDTO.FirstName,
				LastName = registerationRequestDTO.LastName,
				Password = registerationRequestDTO.Password,
				Role= registerationRequestDTO.Role
			};

			_db.LocalUsers.Add(user);
			await _db.SaveChangesAsync();
			user.Password = "";
			return user;

			//try
			//{
			//	var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
			//	if (result.Succeeded)
			//	{
			//		if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
			//		{
			//			await _roleManager.CreateAsync(new IdentityRole("admin"));
			//			await _roleManager.CreateAsync(new IdentityRole("customer"));
			//		}
			//		await _userManager.AddToRoleAsync(user, "admin");
			//		var userToReturn = _db.ApplicationUsers
			//			.FirstOrDefault(u => u.UserName == registerationRequestDTO.UserName);
			//		return _mapper.Map<UserDTO>(userToReturn);

			//	}
			//}
			//catch (Exception e)
			//{

			//}

			//return new UserDTO();
		}
	}
}