﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnSale.Common.Enums;
using OnSale.Common.Models;
using OnSale.Web.Data;
using OnSale.Web.Data.Entities;
using OnSale.Web.Models;
using System;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(
            DataContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password) => await _userManager.CreateAsync(user, password);

        public async Task AddUserToRoleAsync(User user, string roleName) => await _userManager.AddToRoleAsync(user, roleName);

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                await _roleManager.CreateAsync(new IdentityRole{ Name = roleName });
        }

        public async Task<User> GetUserAsync(string email) => await _context.Users.Include(u => u.City).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> IsUserInRoleAsync(User user, string roleName) => await _userManager.IsInRoleAsync(user, roleName);

        public async Task<SignInResult> LoginAsync(LoginViewModel model) => await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password) => await _signInManager.CheckPasswordSignInAsync(user, password, false);

        public async Task<User> AddUserAsync(AddUserViewModel model, string imageId, UserType userType)
        {
            User user = new User
            {
                Address = model.Address,
                RFC = model.RFC,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageId = imageId,
                PhoneNumber = model.PhoneNumber,
                City = await _context.Cities.FindAsync(model.CityId),
                UserName = model.Username,
                UserType = userType,
                Latitude = model.Latitude,
                Logitude = model.Logitude
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result != IdentityResult.Success)
                return null;

            User newUser = await GetUserAsync(model.Username);
            await AddUserToRoleAsync(newUser, user.UserType.ToString());
            return newUser;
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword) => await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        public async Task<IdentityResult> UpdateUserAsync(User user) => await _userManager.UpdateAsync(user);

        public async Task<User> GetUserAsync(Guid userId) => await _context.Users.Include(u => u.City).FirstOrDefaultAsync(u => u.Id == userId.ToString());

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token) => await _userManager.ConfirmEmailAsync(user, token);

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user) => await _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<string> GeneratePasswordResetTokenAsync(User user) => await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password) => await _userManager.ResetPasswordAsync(user, token, password);

        public async Task<User> AddUserAsync(FacebookProfile model)
        {
            User userEntity = new User
            {
                Address = "...",
                RFC = "...",
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageFacebook = model.Picture?.Data?.Url,
                PhoneNumber = "...",
                City = await _context.Cities.FirstOrDefaultAsync(),
                UserName = model.Email,
                UserType = UserType.User,
                LoginType = LoginType.Facebook,
                ImageId = string.Empty
            };

            IdentityResult result = await _userManager.CreateAsync(userEntity, model.Id);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            User newUser = await GetUserAsync(model.Email);
            await AddUserToRoleAsync(newUser, userEntity.UserType.ToString());
            return newUser;
        }

    }
}
