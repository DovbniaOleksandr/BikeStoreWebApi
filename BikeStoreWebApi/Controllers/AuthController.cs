using AutoMapper;
using BikeStore.Core.DTOs.User;
using BikeStore.Core.Enums;
using BikeStore.Core.Models;
using BikeStoreWebApi.DTOs;
using BikeStoreWebApi.DTOs.User;
using BikeStoreWebApi.Helpers;
using BikeStoreWebApi.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly IMapper _mapper;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper, IOptions<AuthOptions> authOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            this._authOptions = authOptions;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if(user == null)
            {
                return NotFound(id);
            }

            var userDto = _mapper.Map<User, UserDto>(user);

            return Ok(userDto);
        }

        [HttpPost("login")]
        [EnableRateLimiting("Auth")]
        public async Task<ActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Unauthorized(Responses.AuthFail);

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

            if(result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var token = JWT.GenerateJWT(user, userRoles, _authOptions.Value);

                var response = new LoginResponse()
                {
                    Roles = userRoles,
                    UserName = user.UserName,
                    Token = token,
                    UserId = user.Id
                };

                return Ok(response);
            }

            return Unauthorized(result.IsLockedOut ? "Your account was locked out." : Responses.AuthFail);
        }

        [HttpPost("register")]
        [EnableRateLimiting("Auth")]
        public async Task<ActionResult> Register([FromBody] RegistrationUserDto registrationUserDto)
        {
            var user = _mapper.Map<RegistrationUserDto, User>(registrationUserDto);

            var result = await _userManager.CreateAsync(user, registrationUserDto.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, registrationUserDto.Role);

                var userDto = _mapper.Map<User, UserDto>(user);

                return Ok(userDto);
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = Roles.Admin, AuthenticationSchemes = AuthSchemes.JwtBearer)]
        [HttpPost("register_admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] AdminRegistrationDto registrationUserDto)
        {
            var user = _mapper.Map<AdminRegistrationDto, User>(registrationUserDto);

            var result = await _userManager.CreateAsync(user, registrationUserDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, registrationUserDto.Role);

                var userDto = _mapper.Map<User, UserDto>(user);

                return Ok(userDto);
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("edit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> EditUser(EditUserDto model)
        {
            var loggedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!User.IsInRole(Roles.Admin) && loggedUserId != model.Id.ToString())
            {
                return StatusCode(403, "You are not allowed to edit this user's information.");
            }

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.UserName = model.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }
    }
}
