﻿using BikeStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Core.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmailAndPassword(string email, string passwd);
        Task<User> GetUserByEmail(string email);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> AddUser(User user);
        Task<User> AddUserToRole(int userId, string role);
        Task UpdateUser(User userToBeUpdated, User user);
        Task DeleteUser(User user);
        string GenerateJWT(User user, AuthOptions authOptions);
    }
}
