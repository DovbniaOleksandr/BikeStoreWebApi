using AutoMapper;
using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreWebApi.DTOs;
using BikeStoreWebApi.Validators;
using Microsoft.AspNetCore.Http;
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
        private readonly IUserService _userService;
        private readonly IOptions<AuthOptions> authOptions;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, IOptions<AuthOptions> authOptions, IMapper mapper)
        {
            _userService = userService;
            this.authOptions = authOptions;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _userService.GetUserByEmailAndPassword(request.Email, request.Password);

            if(user != null)
            {
                var token = _userService.GenerateJWT(user, authOptions.Value);

                return Ok(new { access_token = token, roles = _mapper.Map<ICollection<Role>, ICollection<RoleDto>>(user.Roles) });
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegistrationDto request)
        {
            var validator = new RegistationValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var user = _mapper.Map<RegistrationDto, User>(request);

            var registeredUser = await _userService.AddUser(user);

            registeredUser = await _userService.AddUserToRole(registeredUser.Id, request.Role);

            var userDto = _mapper.Map<User, UserDto>(registeredUser);

            return Ok(userDto);
        }
    }
}
