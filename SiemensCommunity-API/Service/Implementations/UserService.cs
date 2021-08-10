﻿using Data.Contracts;
using Service.Adapters;
using Service.Contracts;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserDTOAdapter _userDTOAdapter = new UserDTOAdapter();

        private readonly IUserRepository _userReposistory; 
        public UserService(IUserRepository userReposistory)
        {
            _userReposistory = userReposistory;
        }

        public async Task<IEnumerable<UserDTO>> GetAsync()
        {
            var returnedUsers = await _userReposistory.GetAsync();
            return _userDTOAdapter.AdaptList(returnedUsers);
        }
    }
}