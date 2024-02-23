using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Verba.Abstractions.Application.Authorization;
using Verba.Core.Application.Authorization;
using Verba.Core.Application.JwtConfiguration;
using Verba.Identity.Core.Queries;
using Verba.Identity.Domain.Account;
using Verba.Identity.Domain.Models;

namespace Verba.Identity.Core.Controllers;

[Route("api/identity")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountQueries _queries;

    public AuthenticationController(UserManager<User> userManager,
        IConfiguration configuration, IHttpContextAccessor contextAccessor,
        IJwtTokenService jwtTokenService, RoleManager<Role> roleManager,
        IAccountQueries queries)
    {
        _userManager = userManager;
        _configuration = configuration;
        _contextAccessor = contextAccessor;
        _jwtTokenService = jwtTokenService;
        _roleManager = roleManager;
        _queries = queries;

        //userManager.UserValidators.Clear();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequestDto)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _userManager.FindByEmailAsync(userLoginRequestDto.Login);
            if (existingUser == null)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    },
                    Result = false
                });

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, userLoginRequestDto.Password);

            if(!isCorrect)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid credentials"
                    },
                    Result = false
                });


            var roles = await _userManager.GetRolesAsync(existingUser);

            var jwtToken = _jwtTokenService.GenerateJwtAuthTokenAsync(existingUser.Email, existingUser.Id, roles);
            _contextAccessor.AddJwtAuthTokenInCookie(jwtToken);

            return Ok(new AuthResult()
            {
                Result = true,
                Token = jwtToken
            });
        }
        return BadRequest(new AuthResult()
        {
            Errors = new List<string>()
            {
                "Invalid payload"
            },
            Result = false
        });
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Any)]
    [Route("logout")]
    public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string jwt)
    {
        var email = _jwtTokenService.GetUserEmailFromJwtAuthToken(jwt);
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser == null)
            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                    {
                        "Invalid JWT"
                    },
                Result = false
            });
        Response.Cookies.Delete("access_token");
        return Ok();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Any)]
    [Route("password_update")]
    public async Task<IActionResult> UpdatePassword([FromHeader(Name = "Authorization")] string jwt, [FromBody] UserPasswordChangeRequest updatedPassword)
    {
        if (ModelState.IsValid)
        {
            var email = _jwtTokenService.GetUserEmailFromJwtAuthToken(jwt);
            if (string.IsNullOrEmpty(email))
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Bad JWT payload"
                    },
                    Result = false
                });

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Can't find user with your login credentials"
                    },
                    Result = false
                });

            var check = await _userManager.CheckPasswordAsync(existingUser, updatedPassword.NewPassword);

            if (check)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Change new password (it matches with an old one)"
                    },
                    Result = false
                });

            var tokenPassword = await _userManager.GeneratePasswordResetTokenAsync(existingUser);

            var result = await _userManager.ResetPasswordAsync(existingUser, tokenPassword, updatedPassword.NewPassword);

            var tokenEmail = await _userManager.GenerateEmailConfirmationTokenAsync(existingUser);

            var unlock = await _userManager.ConfirmEmailAsync(existingUser, tokenEmail);

            if (result.Succeeded && unlock.Succeeded)
                return Ok(new AuthResult()
                {
                    Result = true
                });
            else
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Can't change password or confirm user"
                    },
                    Result = false
                });
        }
        return BadRequest(new AuthResult()
        {
            Errors = new List<string>()
            {
                "Invalid payload"
            },
            Result = false
        });
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Admin)]
    [Route("register_user")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest userRegistrationRequestDto)
    {
        if (ModelState.IsValid)
        {
            var userExists = await _userManager.FindByEmailAsync(userRegistrationRequestDto.Login);
            if (userExists != null)
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "User already exists"
                    }
                });

            var newUser = new User()
            {
                Email = userRegistrationRequestDto.Login,
                UserName = userRegistrationRequestDto.Login,
                EmailConfirmed = false,
                PhoneNumber = userRegistrationRequestDto.PhoneNumber,
                CreatedDatetime = DateTime.UtcNow
            };

            var ss = _userManager.UserValidators.ToList();
            var isCreated = await _userManager.CreateAsync(newUser, userRegistrationRequestDto.Password);
            await AddApplicantRoleToUser(newUser);

            if (isCreated != null)
                return Ok(new AuthResult()
                {
                    Result = true
                });

            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "Server failed"
                }
            });
        }

        return BadRequest();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Admin)]
    [Route("register_uploader")]
    public async Task<IActionResult> RegisterUploader([FromBody] UserRegistrationRequest userRegistrationRequestDto)
    {
        if (ModelState.IsValid)
        {
            var userExists = await _userManager.FindByEmailAsync(userRegistrationRequestDto.Login);
            if (userExists != null)
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "User already exists"
                    }
                });

            var newUser = new User()
            {
                Email = userRegistrationRequestDto.Login,
                UserName = userRegistrationRequestDto.Login,
                EmailConfirmed = false,
                PhoneNumber = userRegistrationRequestDto.PhoneNumber,
                CreatedDatetime = DateTime.UtcNow
            };

            var isCreated = await _userManager.CreateAsync(newUser, userRegistrationRequestDto.Password);
            await AddApplicantRoleToUploader(newUser);

            if (isCreated != null)
                return Ok(new AuthResult()
                {
                    Result = true
                });

            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "Server failed"
                }
            });
        }

        return BadRequest();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Any)]
    [Route("account")]
    public async Task<IActionResult> GetAccountInfo([FromHeader(Name = "Authorization")] string jwt)
    {
        var email = _jwtTokenService.GetUserEmailFromJwtAuthToken(jwt);
        if (string.IsNullOrEmpty(email))
            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                    {
                        "Bad JWT payload"
                    },
                Result = false
            });

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser == null)
            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                    {
                        "Invalid payload"
                    },
                Result = false
            });

        var id = _jwtTokenService.GetUserIdFromJwtAuthToken(jwt);
        var ss = await _userManager.FindByIdAsync(id);
        if (string.IsNullOrEmpty(id) || ss == null)
            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                    {
                        "Invalid JWT"
                    },
                Result = false
            });
        var outcome = await _queries.GetAccountInfoAsync(id);

        return Ok(outcome);
    }

    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Any)]
    [Route("delete_user")]
    public async Task<IActionResult> DeleteAccount([FromHeader(Name = "Authorization")] string jwt)
    {
        var userId = _jwtTokenService.GetUserIdFromJwtAuthToken(jwt);

        if (userId == null)
            return NotFound($"User with ID {userId} not found.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound($"User with ID {userId} not found.");

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
            return NoContent();

        return BadRequest($"Failed to delete user. Errors: {string.Join(", ", result.Errors)}");
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Permissions.Admin)]
    [Route("used_accounts")]
    public async Task<IActionResult> GetUsedAccountsInfo([FromHeader(Name = "Authorization")] string jwt)
    {
        var email = _jwtTokenService.GetUserEmailFromJwtAuthToken(jwt);
        if (string.IsNullOrEmpty(email))
            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                    {
                        "Bad JWT payload"
                    },
                Result = false
            });

        return Ok(await _queries.GetAllAccountsAsync());
    }

    private async Task<Role> AddApplicantRoleToUser(User user)
    {
        var role = await _roleManager.FindByNameAsync(Roles.User);
        var result = await _userManager.AddToRoleAsync(user, role.Name);
        if (!result.Succeeded)
            throw new ApplicationException(string.Join(";", result.Errors.Select(e => e.Description)));
        return role;
    }

    private async Task<Role> AddApplicantRoleToUploader(User user)
    {
        var role = await _roleManager.FindByNameAsync(Roles.Uploader);
        var result = await _userManager.AddToRoleAsync(user, role.Name);
        if (!result.Succeeded)
            throw new ApplicationException(string.Join(";", result.Errors.Select(e => e.Description)));
        return role;
    }
}

