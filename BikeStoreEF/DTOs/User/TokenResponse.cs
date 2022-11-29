using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStoreWebApi.DTOs.User
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
