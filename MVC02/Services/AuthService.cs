using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC02.Models.ViewModels;

namespace Mvc02.Services
{
    public class AuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<bool> AddRoleToUser(string email, string roleName)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        internal async Task<IEnumerable<UserAndRoles>> GetUsersWithRoles()
        {
            var resultList =  GetUsers().Select( x => new UserAndRoles() { User = x, Roles = _signInManager.UserManager.GetRolesAsync(x).Result });

            return resultList;
        }

        
        internal string GetUserId(ClaimsPrincipal claims)
        {
            return  _userManager.GetUserId(claims);
        }

        internal IEnumerable<IdentityUser> GetUsers()
        {
            return _userManager.Users.ToList();
        }

        internal async Task<bool> AddRole(string roleName)
        {
            if (await _roleManager.FindByNameAsync(roleName) != null)
            {
                return false;
            }

            IdentityRole role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        internal async Task<bool> UserExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return !(user == null);
        }

        internal async Task<bool> RoleExists(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }
        // din kod här
    }
}