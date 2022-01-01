using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreWebApi.DTOs;
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

        public AuthController(IUserService userService, IOptions<AuthOptions> authOptions)
        {
            _userService = userService;
            this.authOptions = authOptions;
        }

        [HttpPost("")]
        public async Task<ActionResult> Login([FromBody] Login request)
        {
            var user = await _userService.GetUserByEmailAndPassword(request.Email, request.Password);

            if(user != null)
            {
                var token = _userService.GenerateJWT(user, authOptions.Value);

                return Ok(new { access_token = token });
            }

            return Unauthorized();
        }
    }
}
