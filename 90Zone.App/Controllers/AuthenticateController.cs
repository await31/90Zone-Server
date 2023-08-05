using _90Zone.BusinessObjects.Models;
using _90Zone.App.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _90Zone.App.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase {

        private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            //RoleManager<IdentityRole> roleManager,
            IConfiguration configuration) {
            _userManager = userManager;
            //_roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto model) {
            if (ModelState.IsValid) {

                var userExists = await _userManager.FindByEmailAsync(model.Email);

                if (userExists != null) {
                    return BadRequest(new AuthResult() {
                        Result = false,
                        Errors = new List<string>() {
                            "Email already existed! Please use another email"
                        }
                    });
                }

                var newUser = new IdentityUser() {
                    UserName = model.Email,
                    Email = model.Email
                };

                var isCreated = await _userManager.CreateAsync(newUser, model.Password);

                if (isCreated.Succeeded) {
                    var token = GenerateToken(newUser);
                    return Ok(new AuthResult() {
                        Result = true,
                        Token = token
                    });
                } else {
                    return BadRequest(new AuthResult() {
                        Errors = new List<string>() {
                            "Server error"
                        },
                        Result = false
                    });
                }

            }
            return BadRequest();
        }

        private string GenerateToken(IdentityUser user) {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
