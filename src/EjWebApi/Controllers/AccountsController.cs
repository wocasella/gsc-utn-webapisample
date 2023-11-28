using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EjWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ClaveDeSeguridadConUnMínimoDe256Bits"));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: new[] { new Claim(ClaimTypes.Name, "admin") },
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            string jwt = tokenHandler.WriteToken(securityToken);

            return this.Ok(jwt);
        }
    }
}
