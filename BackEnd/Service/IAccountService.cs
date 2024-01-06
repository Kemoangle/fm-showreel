using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.Service
{
    public interface IAccountService
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);
        public Task<string> SignInAsync(SignInModel signInModel);
    }
}