using Microsoft.AspNetCore.Mvc;
using MyReet.Models;
using System.Data;
using System.Data.SqlClient;

namespace MyReet.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class RegistrationController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public RegistrationController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpPost]
		[Route("registration")]

		public string registration(Registration registration)
		{
			SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultSQLConnection").ToString());

			SqlCommand cmd = new SqlCommand("INSERT INTO Registration(UserName,Password,Email,IsActive) VALUES('" + registration.UserName + "','" + registration.Password + "','" + registration.Email + "','" + registration.IsActive + "')", con);
			con.Open();
			var i = cmd.ExecuteNonQuery();
			con.Close();
			if (i > 0)
			{
				return "Data Inserted";
			}
			else
			{
				return "ERROR";
			}

		}

		[HttpPost]
		[Route("login")]

		public string login(Registration registration)
		{
			SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultSQLConnection").ToString());

			SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Registration WHERE Email = '" + registration.Email + "' AND Password = '" + registration.Password + "' AND IsActive = 1 ", con);

			DataTable dt = new DataTable();
			da.Fill(dt);
			if (dt.Rows.Count > 0)
			{
				return "Valid User";
			}
			else
			{
				return "Invalid User";
			}
		}

		[HttpPost]
		[Route("logout")]
		public string Logout()
		{
			string connectionString = _configuration.GetConnectionString("DefaultSQLConnection").ToString();
			string updateQuery = "UPDATE Registration SET IsActive = 1 WHERE Email = @Email";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand command = new SqlCommand(updateQuery, connection))
				{
					command.Parameters.AddWithValue("@Email", "aa@gmail.com");

					connection.Open();
					int rowsAffected = command.ExecuteNonQuery();
					connection.Close();

					if (rowsAffected > 0)
					{
						return "Logout successful";
					}
					else
					{
						return "Logout failed";
					}
				}
			}
		}

	}
}
