using MyReet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MyReet.Data
{
	//IdentityDbContext<ApplicationUser>
	public class ApplicationDbContext : DbContext
	{

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		//public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<LocalUser> LocalUsers { get; set; }

		

	}
}
