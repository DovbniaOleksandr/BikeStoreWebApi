using BikeStore.Core.Enums;
using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStoreEF;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<AuthOptions> authOptions;

        public UserService(IUnitOfWork unitOfWork, IOptions<AuthOptions> authOptions)
        {
            this._unitOfWork = unitOfWork;
            this.authOptions = authOptions;
        }

        public async Task<User> AddUser(User user)
        {
            if ((await GetUserByEmail(user.Email)) != null)
                throw new ArgumentException("Email '" + user.Email + "' is already taken");

            user.Password = HashPassword(user.Password);

            await _unitOfWork.Users.AddAsync(user);

            await _unitOfWork.SaveAsync();

            return user;
        }

        public async Task<User> AddUserToRole(int userId, string role)
        {
            var user = await _unitOfWork.Users.GetByIdWithRoles(userId);

            var existingRole = _unitOfWork.Roles.Find(r => r.Name == role).FirstOrDefault();

            if (existingRole == null)
            {
                existingRole = new Role()
                {
                    Name = role
                };

                await _unitOfWork.Roles.AddAsync(existingRole);
            }

            user.Roles.Add(existingRole);

            await _unitOfWork.SaveAsync();
            return user;
        }

        public async Task DeleteUser(User user)
        {
            _unitOfWork.Users.Remove(user);
            await _unitOfWork.SaveAsync();
        }

        private string HashPassword(string password)
        {
            var inputBytes = Encoding.UTF8.GetBytes(password + authOptions.Value.Secret);

            var hashedBytes = new SHA256CryptoServiceProvider().ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }

        public string GenerateJWT(User user, AuthOptions authOptions)
        {
            var securityKey = authOptions.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim("role", role.Name));
            }

            var token = new JwtSecurityToken(authOptions.Issuer,
                authOptions.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authOptions.TokenLifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _unitOfWork.Users.GetAllWithRoles();
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string passwd)
        {
            return await _unitOfWork.Users.GetByEmailAndPasswordWithRoles(email, HashPassword(passwd));
        }

        public async Task<User> GetUserById(int id)
        {
            return await _unitOfWork.Users.GetByIdWithRoles(id);
        }

        public async Task UpdateUser(User userToBeUpdated, User user)
        {
            userToBeUpdated.Email = user.Email;
            userToBeUpdated.Password = HashPassword(user.Password);

            await _unitOfWork.SaveAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
