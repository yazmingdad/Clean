using Clean.Core.Models.Auth;
using Clean.Core.Models.Company;
using Clean.Infrastructure.CleanDb.Models;
using Clean.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clean.Auth.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<UserModel> GetUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _userService.GetByIdAsync(userId);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route("up")]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_userService.GetUsers());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route("roles")]
        public IActionResult GetRoles()
        {

            try
            {
                return Ok(_userService.GetRoles());
            }
            catch(Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> InsertAsync(UserInsertModel userModel)
        {
            var output = await _userService.InsertAsync(userModel);

            if (output.IsFailure)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [Route("addrole")]
        public async Task<IActionResult> AddRoleAsync(UserRoleModel userModel)
        {
            string byUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var output = await _userService.AddRoleAsync(byUserId, userModel);

            if (output.IsFailure)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [Route("removerole")]
        public async Task<IActionResult> RemoveRoleAsync(UserRoleModel userModel)
        {
            string byUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var output = await _userService.RemoveRoleAsync(byUserId, userModel);

            if (output.IsFailure)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [Route("disableuser")]

        public IActionResult DisableUserAsync(UserIdModel userId)
        {
            string byUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var output = _userService.DisableUser(byUserId, userId.UserId);

            if (output.IsFailure)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
