using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using SafeVaultAPI.Models;
using BCrypt.Net;

namespace SafeVaultAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        // 🔐 Input Sanitization
        private string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            input = Regex.Replace(input, "<.*?>", "");       // Remove HTML tags
            input = Regex.Replace(input, @"[^\w@\.\-]", ""); // Allow safe chars

            return input;
        }

        // 🟢 REGISTER
        [HttpPost("/register")]
        public IActionResult Register(User user)
        {
            string username = SanitizeInput(user.Username ?? "");
            string email = SanitizeInput(user.Email ?? "");

            if (string.IsNullOrEmpty(user.Password))
                return BadRequest("Password is required");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            string connStr = _config.GetConnectionString("DefaultConnection") ?? "";

            if (connStr != "dummy")
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"INSERT INTO Users 
                                    (Username, Email, PasswordHash, Role) 
                                    VALUES (@Username, @Email, @PasswordHash, @Role)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        cmd.Parameters.AddWithValue("@Role", "user");

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            return Ok("User registered successfully");
        }

        // 🟢 LOGIN
        [HttpPost("/login")]
        public IActionResult Login(User user)
        {
            string originalUsername = user.Username ?? "";
            string cleanUsername = SanitizeInput(originalUsername);

            // 🚨 IMPORTANT SECURITY CHECK
            if (originalUsername != cleanUsername)
            {
                return Unauthorized("Malicious input detected");
            }

            string connStr = _config.GetConnectionString("DefaultConnection") ?? "";

            string storedHash = "";
            string role = "";

            if (connStr != "dummy")
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = "SELECT PasswordHash, Role FROM Users WHERE Username=@Username";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", cleanUsername);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                storedHash = reader.GetString("PasswordHash");
                                role = reader.GetString("Role");
                            }
                            else
                            {
                                return Unauthorized("User not found");
                            }
                        }
                    }
                }
            }
            else
            {
                // Dummy for testing
                storedHash = BCrypt.Net.BCrypt.HashPassword("test123");
                role = "user";
            }

            if (!BCrypt.Net.BCrypt.Verify(user.Password, storedHash))
            {
                return Unauthorized("Invalid password");
            }

            return Ok(new { message = "Login successful", role = role });
        }

        // 🟢 ADMIN
        [HttpGet("/admin")]
        public IActionResult AdminDashboard([FromHeader] string role)
        {
            if (role != "admin")
            {
                return Forbid("Access denied");
            }

            return Ok("Welcome Admin!");
        }

        // 🟢 USER
        [HttpGet("/user")]
        public IActionResult UserDashboard()
        {
            return Ok("Welcome User!");
        }
    }
}