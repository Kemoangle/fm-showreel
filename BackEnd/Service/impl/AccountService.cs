using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Showreel.Models;

namespace BackEnd.Service.impl
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ShowreelContext context;
        public AccountService(
            UserManager<AppUser> _userManager,
            SignInManager<AppUser> _signInManager,
            IConfiguration _configuration,
            ShowreelContext _context
        )
        {
            userManager = _userManager;
            signInManager = _signInManager;
            configuration = _configuration;
            context = _context;
        }
        public async Task<string> SignInAsync(SignInModel signInModel)
        {
            var result  = await signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);
            if(!result.Succeeded){
                return string.Empty;
            }
            var authClaims = new List<Claim>{
                new Claim(ClaimTypes.Email, signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                audience : configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel signUpModel)
        {
            var user = new AppUser {
                Email = signUpModel.Email,
                UserName = signUpModel.Email
            };
            return await userManager.CreateAsync(user, signUpModel.Password);
        }
    }
}