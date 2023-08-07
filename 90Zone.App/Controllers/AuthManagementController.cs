using _90Zone.BusinessObjects.Models;
using _90Zone.App.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Rewrite;

namespace _90Zone.App.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthManagementController> _logger;
        private readonly IConfiguration _configuration;

        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            TokenValidationParameters tokenValidationParams,
            ILogger<AuthManagementController> logger,
            IConfiguration configuration) {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
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
                    UserName = model.Username,
                    Email = model.Email
                };

                var isCreated = await _userManager.CreateAsync(newUser, model.Password);

                if (isCreated.Succeeded) {

                    await _userManager.AddToRoleAsync(newUser, "Admin");

                    var token = await GenerateToken(newUser);

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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto model) {
            if (ModelState.IsValid) {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser == null) {
                    return BadRequest(new AuthResult() {
                        Errors = new List<string> {
                    "Invalid payload"
                },
                        Result = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, model.Password);

                if (!isCorrect) {
                    return BadRequest(new AuthResult() {
                        Errors = new List<string> {
                    "Invalid credentials"
                },
                        Result = false
                    });
                }

                var jwtToken = await GenerateToken(existingUser);

                return Ok(new AuthResult() {
                    Token = jwtToken,
                    Result = true
                });
            }
            return BadRequest( new AuthResult() {
                Errors = new List<string> {
                    "Invalid payload"
                },
                Result = false
            });
        }

        private async Task<string> GenerateToken(IdentityUser user) {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value);

            var claims = await GetAllValidClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        //Get all valid claims for the corresponding user
        private async Task<List<Claim>> GetAllValidClaims(IdentityUser user) {

            var _options = new IdentityOptions();

            var claims = new List<Claim>
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                };

            //Getting the claims that we assigned to the user
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            //Get the user role and add it to the claims
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var userRole  in userRoles) {
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null) {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach(var roleClaim in roleClaims) {
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }
    }
}
