﻿namespace Service.Models
{
    public class UserLoginCredentials
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}
