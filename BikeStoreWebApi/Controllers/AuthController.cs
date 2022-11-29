using AutoMapper;
using BikeStore.Core.Enums;
using BikeStore.Core.Models;
using BikeStoreWebApi.DTOs;
using BikeStoreWebApi.DTOs.User;
using BikeStoreWebApi.Helpers;
using BikeStoreWebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            if (user == null)
                return Unauthorized("Wrong user name.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

            if(result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var token = JWT.GenerateJWT(user, userRoles, _authOptions.Value);

                var refreshToken = JWT.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_authOptions.Value.RefreshTokenLifetime);

                await _userManager.UpdateAsync(user);

                var response = new LoginResponse()
                {
                    Roles = userRoles,
                    UserName = user.UserName,
                    Token = token,
                    UserId = user.Id,
                    RefreshToken = refreshToken
                };

                return Ok(response);
            }

            return Unauthorized(result.IsLockedOut ? "Your account was locked out." : string.Empty);
        }

        [HttpPost("register")]
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

        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh(Token token)
        {
            if (token is null)
                return BadRequest("Invalid client request");

            string accessToken = token.AccessToken;
            string refreshToken = token.RefreshToken;

            var principal = JWT.GetPrincipalFromExpiredToken(accessToken, _authOptions.Value);
            var email = principal.FindFirst(ClaimTypes.Email).Value;
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            var userRoles = await _userManager.GetRolesAsync(user);
            var newAccessToken = JWT.GenerateJWT(user, userRoles, _authOptions.Value);
            var newRefreshToken = JWT.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new TokenResponse()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
