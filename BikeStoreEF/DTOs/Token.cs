using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStoreWebApi.DTOs
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
