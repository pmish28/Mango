﻿namespace Mango.Services.AuthAPI.Model
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

    }
}
