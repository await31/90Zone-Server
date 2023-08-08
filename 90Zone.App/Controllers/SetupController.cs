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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using _90Zone.Repositories;

namespace _90Zone.App.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class SetupController : ControllerBase {

        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<SetupController> _logger;

        public SetupController(
            IUserRepository userRepository,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<SetupController> logger) 
        {
            _userRepository = userRepository;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetAllRole() { 
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public IActionResult GetAllRoles() {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers() {
            var users = _userRepository.GetUsers();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string email) {

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) {
                _logger.LogInformation($"The user with email {email} is not exist!");
                return Ok(new {
                    result = $"The user with email {email} is not exist!"
                });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(string name) {
            var roleExist = await _roleManager.RoleExistsAsync(name);
            if (!roleExist) {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));
                if (roleResult.Succeeded) {
                    _logger.LogInformation($"The role {name} has been added successfully!");
                    return Ok(new {
                        result = $"The role {name} has been added successfully!"
                    });
                } else {
                    _logger.LogInformation($"The role {name} has not been added!");
                    return BadRequest(new {
                        error = $"The role {name} has not been added!"
                    });
                }
            }
            return BadRequest(new { error = "Role already existed" });
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName) {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                _logger.LogInformation($"The user with email {email} is not exist!");
                return BadRequest(new {
                    error = $"User does not exist!"
                });
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist) {
                _logger.LogInformation($"The role {roleName} is not exist!");
                return BadRequest(new {
                    error = $"Role does not exist!"
                });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded) {
                return Ok(new {
                    result = $"The user with email {email} has been add to role {roleName} successfully!"
                });
            } else {
                _logger.LogInformation($"The user was not able to be added to the role!");
                return BadRequest(new {
                    error = $"The user was not able to be added to the role!"
                });
            }
        }

        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName) {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                _logger.LogInformation($"The user with email {email} is not exist!");
                return BadRequest(new {
                    error = $"User does not exist!"
                });
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist) {
                _logger.LogInformation($"The role {roleName} is not exist!");
                return BadRequest(new {
                    error = $"Role does not exist!"
                });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded) {
                return Ok(new {
                    result = $"The user with email {email} has been remove from role {roleName} successfully!"
                });
            } else {
                _logger.LogInformation($"The user was not able to be removed from the role!");
                return BadRequest(new {
                    error = $"The user was not able to be removed from the role!"
                });
            }
        }
    }
}
