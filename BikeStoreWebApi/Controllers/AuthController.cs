using AutoMapper;
using BikeStore.Core.Enums;
using BikeStore.Core.Models;
using BikeStoreWebApi.DTOs.User;
using BikeStoreWebApi.Helpers;
using BikeStoreWebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<UserDto>> GetById(int id)
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
        public async Task<ActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

            if(result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var token = JWT.GenerateJWT(user, userRoles, _authOptions.Value);

                var response = new LoginResponse()
                {
                    Roles = userRoles,
                    UserName = user.UserName,
                    Token = token
                };

                return Ok(response);
            }

            return Unauthorized(result.IsLockedOut ? "Your account was locked out." : string.Empty);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegistrationUserDto registrationUserDto)
        {
            var validator = new UserRegistationValidator();
            var validationResult = await validator.ValidateAsync(registrationUserDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

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
        public async Task<ActionResult> RegisterAdmin([FromBody] RegistrationUserDto registrationUserDto)
        {
            var validator = new AdminRegistationValidator();
            var validationResult = await validator.ValidateAsync(registrationUserDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var user = _mapper.Map<RegistrationUserDto, User>(registrationUserDto);

            var result = await _userManager.CreateAsync(user, registrationUserDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, registrationUserDto.Role);

                var userDto = _mapper.Map<User, UserDto>(user);

                return Ok(userDto);
            }

            return BadRequest(result.Errors);
        }
    }
}
