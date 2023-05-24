using BookStoreApi.Models;
using BookStoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace BookStoreApi.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public TokenController(IConfiguration config, AppDbContext context, IUserRepository userRepository)
        {
            _configuration = config;
            _context = context;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await _userRepository.GetUserAsync(_userData.Email, _userData.Password);

                if (user != null)
                {
                    var token = await _userRepository.GenerateToken(user);
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return new ObjectResult("Не найден пользователь!") { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }
    }
}
