﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC02.Models;
using MVC02.Models.ViewModels;

namespace Mvc02.Services
{
    public class AuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AuthService(UserManager<IdentityUser> userManager, RoleManager<AppRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<bool> AddRoleToUser(string id, string roleName)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        internal async Task<IEnumerable<UserAndRoles>> GetUsersWithRoles()
        {
            var resultList = GetUsers().Select(x => new UserAndRoles() { User = x, Roles = _signInManager.UserManager.GetRolesAsync(x).Result });

            return resultList;
        }
        internal async Task<IEnumerable<SelectListItem>> GetRolesAsSelectListItems()
        {
            var resultList = _roleManager.Roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name });
            return resultList;
        }


        internal string GetUserId(ClaimsPrincipal claims)
        {
            return _userManager.GetUserId(claims);
        }

        internal async Task<IdentityUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
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

            AppRole role = new AppRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        internal IEnumerable<SelectListItem> GetAllRoles()
        {
            return _roleManager.Roles.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
        }

        internal async Task<bool> UserExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return !(user == null);
        }

        internal async Task<IEnumerable<string>> GetUsersRole(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        internal async Task<bool> RoleExists(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        internal async Task DeleteRole(IdentityUser user, IEnumerable<string> rolesToDelete)
        {
            await  _userManager.RemoveFromRolesAsync(user, rolesToDelete);
        }
        // din kod här
    }
}