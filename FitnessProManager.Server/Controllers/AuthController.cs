using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace FitnessProManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string _connectionString;

        public AuthController(IConfiguration configuration)
        {
            // "DefaultConnection" must match your appsettings.json name
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public class RegisterDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto request)
        {
            // 1. Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 2. Save to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Users (FirstName, LastName, Email, PasswordHash) 
                                 VALUES (@FirstName, @LastName, @Email, @PasswordHash)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", request.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", request.LastName);
                    cmd.Parameters.AddWithValue("@Email", request.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        return BadRequest("Email already exists.");
                    }
                }
            }
            return Ok(new { message = "User registered successfully!" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto request)
        {
            string storedHash = "";
            int userId = 0;
            string firstName = "";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Id, FirstName, PasswordHash FROM Users WHERE Email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", request.Email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = (int)reader["Id"];
                            firstName = reader["FirstName"].ToString();
                            storedHash = reader["PasswordHash"].ToString();
                        }
                        else
                        {
                            return BadRequest("User not found.");
                        }
                    }
                }
            }

            // 3. Verify Password
            bool validPassword = BCrypt.Net.BCrypt.Verify(request.Password, storedHash);

            if (!validPassword)
            {
                return BadRequest("Incorrect password.");
            }

            return Ok(new { message = "Login successful!", userId = userId, user = firstName });
        }
    }
}