using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoListApi.Models;
using ToDoListApi.Models.Exceptions;

namespace ToDoListApi.Controllers
{
    [Route("api/log-in")]
    [ApiController]
    public class ExternalAuthenticationController : ControllerBase
    {
        private readonly IStringLocalizer _resourceLocalizer;
        public ExternalAuthenticationController(IStringLocalizer resourceLocalizer)
        {            
            _resourceLocalizer = resourceLocalizer;
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if (loginModel.Login == "admin" && loginModel.Password == "password")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, loginModel.Login)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key-that-must-be-at-least-16-characters-long"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var token = new JwtSecurityToken(
                    issuer: "localhost",
                    audience: "localhost",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                throw new UnauthorizedException(_resourceLocalizer["Unauthorized", loginModel]);
            }
        }
    }
}

