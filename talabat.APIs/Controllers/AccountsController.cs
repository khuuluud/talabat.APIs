using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using talabat.APIs.Dtos;
using talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly ITokenService _tokenService;

        public AccountsController(UserManager<AppUser> userManager , SignInManager<AppUser> signinManager , ITokenService tokenService)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _tokenService = tokenService;
        }
      

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };

         var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            var Returneduser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenasync(user, _userManager)
            };

            return Ok(Returneduser);
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));

            var Result = await _signinManager.CheckPasswordSignInAsync(user, model.Password, false);

            if(!Result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenasync(user, _userManager)
            });
        }


    }



}
