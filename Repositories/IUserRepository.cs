using BookStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreApi.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Поиск пользователя по почте
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<User> GetUserAsync(string email, string password);


        /// <summary>
        /// Получения пользователя по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<User> GetUserAsync(Guid id);


        /// <summary>
        /// Генератор токена
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<JwtSecurityToken> GenerateToken(User user);


    }
}
