using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplePlanningPoker.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;

        }

        /// <summary>
        /// Creates a new token for the given username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateToken([FromBody] string username)
        {
            try
            {
                var token = GenerateJSONWebToken(username);
                return Ok(new { Token = token, Message = "Success" });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest("Failed to create token");
                throw;
            }
        }

        /// <summary>
        /// Generates a new JSON Web Token.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private string GenerateJSONWebToken(string username)
        {
            var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured");
            var issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured");
            var audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is not configured");


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken(issuer,
                audience,
                claims: new[] { new Claim(JwtRegisteredClaimNames.Name, username) },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return tokenHandler.WriteToken(token);
        }
    }
}

