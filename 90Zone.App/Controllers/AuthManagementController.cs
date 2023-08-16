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
using AutoMapper;
using System.Security.Cryptography;
using _90Zone.Repositories;

namespace _90Zone.App.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthManagementController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AuthManagementController(
            IUserRepository userRepository,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            TokenValidationParameters tokenValidationParams,
            ILogger<AuthManagementController> logger,
            IConfiguration configuration) {
            _userRepository = userRepository;
            _mapper = mapper;
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

                    await _userManager.AddToRoleAsync(newUser, "Journalist");

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

                var existingUser = _userRepository.GetUsers().FirstOrDefault(a => a.Email == model.Email);

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

                existingUser.Token = await GenerateToken(existingUser);
                var newAccessToken = existingUser.Token;
                var newRefreshToken = CreateRefreshToken();
                existingUser.RefreshToken = newRefreshToken;
                existingUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                _userRepository.Save();

                return Ok(new TokenApiDto() {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            return BadRequest(new AuthResult() {
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
                Expires = DateTime.Now.AddMinutes(15),
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

            var userName = await _userManager.GetUserNameAsync(user);

            claims.AddRange(userClaims);

            //Get the user role and add it to the claims
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles) {
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null) {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    claims.Add(new Claim(ClaimTypes.Name, userName));
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims) {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }

        private string CreateRefreshToken() {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);
            var tokenInUser = _userRepository.GetUsers()
                .Any(a => a.RefreshToken == refreshToken);
            if (tokenInUser) {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token) {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value);

            var tokenValidationParams = new TokenValidationParameters {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)) {
                throw new SecurityTokenException("This is invalid token!");
            }
            return principal;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody]TokenApiDto tokenApiDto) {
            if (tokenApiDto is null) {
                return BadRequest("Invalid client request!");
            }
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = _userRepository.GetUsers().FirstOrDefault(a => a.UserName == username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) {
                return BadRequest("Invalid request");
            }

            var newAccessToken = await GenerateToken(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            _userRepository.Save();
            return Ok(new TokenApiDto() {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
