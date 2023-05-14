using AutoMapper;
using EmployeeAttendaceSys.Data;
using EmployeeAttendaceSys.Dtos;
using EmployeeAttendaceSys.Entities;
using EmployeeAttendaceSys.Extensions;
using EmployeeAttendaceSys.Interfaces;
using EmployeeAttendaceSys.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeAttendaceSys.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("encrypt")]
        public async Task<ActionResult<UserDto>> EncryptData(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName)) return BadRequest("Username is taken");

            var user = _mapper.Map<User>(registerDto);

            using var hmac = new HMACSHA512();

            user.UserName = registerDto.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            _context.AdminUsers.Add(user);
            await _context.SaveChangesAsync();

            EncrptionService encrptionService = new EncrptionService();

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                EncryptedPassword = encrptionService.EncryptData(registerDto.Password)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            try
            {
                if (await UserExists(registerDto.UserName)) return BadRequest("Username is taken");

                var user = _mapper.Map<User>(registerDto);

                using var hmac = new HMACSHA512();

                user.UserName = registerDto.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
                user.PasswordSalt = hmac.Key;

                _context.AdminUsers.Add(user);
                await _context.SaveChangesAsync();

                Console.WriteLine(user);

                return new UserDto
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex);
            }
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.AdminUsers
                .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user.Status == 0)
            {
                return BadRequest("Unable to access due to user being inactive");
            }

            EncrptionService encrptionService = new EncrptionService();

            var passwordDecrypted = encrptionService.DecryptString(loginDto.Password);
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordDecrypted));

            if (passwordDecrypted == null) return Unauthorized("Unable to find password");


            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            var token = _tokenService.CreateToken(user);


            return new UserDto
            {
                UserId = user.Id.ToString(),
                UserName = user.UserName,
                Token = token,
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.AdminUsers.AnyAsync(X => X.UserName == username.ToLower());
        }
    }
}
