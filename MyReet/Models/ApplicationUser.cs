using Microsoft.AspNetCore.Identity;

namespace MyReet.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
	}
}
