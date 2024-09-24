using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repositories.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DAL.Repository.Services
{
    public class UserServices : IUserServices
    {
        private readonly PeerlandingContext _context;
        private readonly IConfiguration _configuration;
        public UserServices(PeerlandingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> Register(ReqRegisterUserDto register)
        {
            var isAnyEmail = await _context.MstUsers.SingleOrDefaultAsync(x => x.Email == register.Email);
            if (isAnyEmail != null)
            {
                throw new Exception("Email already useddd!");
            }
            var user = new MstUser
            {
                Name = register.Name,
                Email = register.Email,
                Pass = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = register.Role,
                Balance = register.Balance ?? 0,
            };

            await _context.MstUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Name;
        }

        public async Task<ResLoginDto> Login(ReqLoginUserDto login)
        {
            var user = await _context.MstUsers.SingleOrDefaultAsync(x=> x.Email == login.Email);
            if (user == null)
            {
                throw new Exception("Email or Password is Wrong!");
            };
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password, user.Pass);
            
            if (!isPasswordValid)
            {
                throw new Exception("Email or Password is Wrong!");
            }

            var token = GenerateJwtToken(user);

            var loginResp = new ResLoginDto
            {
                Token = token,
            };

            return loginResp;
        }

        public async Task<List<ResUserDto>> GetAllUsers()
        {
            return await _context.MstUsers.Select(user => new ResUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Balance = user.Balance
            }).ToListAsync();
        }

        

        private string GenerateJwtToken(MstUser user)
        {
            var jwtOptions = _configuration.GetSection("jwtSettings");
            var secretKey = jwtOptions["secretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtOptions["validIssuer"],
                audience: jwtOptions["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ResUpdateDto> UpdateUserbyAdmin(ReqUpdateAdminDto reqUpdate, string id)
        {
            var user = _context.MstUsers.SingleOrDefault(x => x.Id == id);
            if (user == null)
            {
                throw new Exception("User not found!");
            }
            
            user.Name = reqUpdate.Name;
            user.Role = reqUpdate.Role;
            user.Balance = reqUpdate.Balance ?? 0;

            var newUser = _context.MstUsers.Update(user).Entity;
            _context.SaveChanges();

            var updateRes = new ResUpdateDto
            {
                nama = newUser.Name,
            };

            return updateRes;
        }

        public async Task<string> Delete(string id)
        {
            var user = _context.MstUsers.SingleOrDefault(e => e.Id == id);

            if (user == null)
            {
                throw new Exception("User not found");
            }


            _context.MstUsers.Remove(user);
            _context.SaveChanges();


            return id;
        }
    }
}