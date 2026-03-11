using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.DTOs;
using RestaurantApi.Helpers;
using RestaurantApi.Models;
using RestaurantApi.Services;


namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController :ControllerBase
    {
        private readonly RestaurantDBContext _context;
        private readonly TokenService _tokenService;

        public AuthController(RestaurantDBContext context , TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = PasswordHasher.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
            if (user == null)
                return Unauthorized("Invalid Username");

            var hashedPassword = PasswordHasher.HashPassword(dto.Password);
            if (user.PasswordHash != hashedPassword)
                return Unauthorized("Invalid password");

            var token = _tokenService.CreateToken(user);

            return Ok(new
            {
                token = token
            }
                );


        }
    }
}
