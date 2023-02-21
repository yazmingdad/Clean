using Clean.Core.Models;
using Clean.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clean.Auth.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IConfiguration _config;

        public TokenController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _config = config;
            _context = context;
            _userManager = userManager;
        }

        [Route("/token")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CredentialsModal credentials)
        {


            if (await IsValidUsernameAndPassword(credentials.Username, credentials.Password))
            {
                return (new ObjectResult(await GenerateToken(credentials.Username)));
            }
            else
            {
                return BadRequest("Wrong Credentials");
            }
        }
        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {
            // check IsDown also

            try
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user == null)
                {
                    return false;
                }

                if (user.IsDown)
                {
                    return false;
                }
                else
                {
                    return await _userManager.CheckPasswordAsync(user, password);
                }
            }
            catch
            {
                return false;
            }
        }

        private async Task<dynamic> GenerateToken(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var roles = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf,new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            string key = _config.GetValue<string>("Secrets:SecurityKey");

            var token = new JwtSecurityToken(
                       new JwtHeader(
                           new SigningCredentials(
                               new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                               SecurityAlgorithms.HmacSha256)
                           ),
                       new JwtPayload(claims));

            var output = new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = username
            };

            return output;
        }
    }
}
