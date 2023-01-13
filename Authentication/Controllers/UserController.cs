using Authentication.Key;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.BindingModel;
using Model.DTO;
using Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JWTConfig _jWTConfig;
        public UserController(ILogger<UserController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            IOptions<JWTConfig> jWTConfig)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _jWTConfig = jWTConfig.Value;
        }

        [HttpPost("RegisterUser")]
        public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
        {
            try
            {
                var user = new AppUser()
                {
                    FullName = model.FullName,
                    UserName = model.Email,
                    Email = model.Email,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return await Task.FromResult("User has been Registered");
                }
                return await Task.FromResult(string.Join(",", result.Errors.Select(x => x.Description).ToArray()));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }
        }

        [HttpGet("GetAllUser")]
        public async Task<object> GetAllUser()
        {
            try
            {
                var user = _userManager.Users.Select(x => new UserDTO(x.FullName, x.Email, x.UserName, x.DateCreated));
                return await Task.FromResult(user);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<object> Login([FromBody] LoginBindingModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Password))
                {
                    return await Task.FromResult("Please enter email or password.");
                }
                else if (string.IsNullOrEmpty(model.Email))
                {
                    return await Task.FromResult("Please enter email.");
                }
                else if (string.IsNullOrEmpty(model.Password))
                {
                    return await Task.FromResult("Please enter password.");
                }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var appUser = await _userManager.FindByNameAsync(model.Email);
                    if (appUser != null)
                    {
                        var user = new UserDTO(appUser.FullName, appUser.Email, appUser.UserName, appUser.DateCreated);
                        user.Token = GenerateToken(appUser);
                        return await Task.FromResult(user);
                    }
                }
                return await Task.FromResult("invalid email or password.");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }
        }
        private string GenerateToken(AppUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jWTConfig.key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
