﻿using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IAccountRepository : IGenericRepository<User>
    {
        public Task<int> RegisterAsync(User user, string password);

        public Task<bool> VerifyLoginAsync(UserLoginCredentials userLoginCredentials);

        public Task<string> ForgotPasswordAsync(string email);
    }
}
