﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.DTOs.User
{
    public class LoginResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
        public string UserName { get; set; }
    }
}
