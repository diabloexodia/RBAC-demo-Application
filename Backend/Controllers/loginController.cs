using jwtvalidator.Repository.Interfaces;
using jwtvalidator.Services;
using jwtvalidator.Models;
using Microsoft.AspNetCore.Mvc;
using jwtvalidator.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace jwtvalidator.Controllers
{
    

    public class loginController : Controller
    {
        private userService _userService;
        private readonly IConfiguration configuration;
        
        public loginController(userService userService, IConfiguration configuration)
        {
            _userService = userService;
            this.configuration = configuration; 

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("login")] // Indicate a POST request
        public async Task<IActionResult> Login([FromBody] loginModelForm model) // Receive the JSON object
        {
          
            try
            {
                bool valid = await _userService.validate(model.username, model.password);

                if (valid)
                {
                    Claim adminClaim = new Claim("role", "admin");
                    Claim audClaim = new Claim("aud", "http://localhost:4200");
                    Claim issuerClaim = new Claim("iss", "https://localhost:7160");
                    List<Claim> claims = new List<Claim>
            {
                adminClaim,audClaim,issuerClaim
            };



                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                    var c = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"])), SecurityAlgorithms.HmacSha256Signature);
                    var expiry = DateTime.UtcNow.AddDays(1); // Set expiration time as needed
                   

                    var token = new JwtSecurityToken(claims: claims,expires: expiry, signingCredentials: c);

                  

                    // Return the token as a response
                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during login: " + ex.Message);
                return StatusCode(500, new { status = "error" });
            }
        }


         [Authorize(Roles = "admin")]
        [HttpGet("example")]
        public IActionResult Example()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();


            Console.WriteLine($"JWT Token: {token}");

            //return Ok(new { body = "admin authorized" });
            return Ok();

        }
     

    }




}