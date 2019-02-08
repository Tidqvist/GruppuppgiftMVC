﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc02.Services;
using MVC02.Data;
using MVC02.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC02.Controllers
{
    //[Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _auth;
        private readonly IHostingEnvironment _env;

        public AdminController(ApplicationDbContext context, AuthService auth, IHostingEnvironment env)
        {
            _context = context;
            _auth = auth;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<UserAndRoles> usersWithRoles = await _auth.GetUsersWithRoles();
            return View(usersWithRoles);
        }

        public async Task<IActionResult> ManageRoles()
        {
            IEnumerable<UserAndRoles> usersWithRoles = await _auth.GetUsersWithRoles();
            return View(usersWithRoles);
        }

        public async Task<IActionResult> AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string role)
        {
            if (!await _auth.AddRole(role))
            {
                ModelState.AddModelError("RoleExist", "Rollen finns redan");
                return View();
            }
            return View();
        }

        public async Task<IActionResult> AddRoleForUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleForUser(AddRoleVm addrole)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (!await _auth.UserExist(addrole.Email))
            {
                ModelState.AddModelError("UserExist", "Användaren hittades inte");
                return View();
            }
            else if (!await _auth.RoleExists(addrole.Role))
            {
                ModelState.AddModelError("RoleExist", "Rollen hittades inte");
                return View();
            }
            else if (!await _auth.AddRoleToUser(addrole.Email, addrole.Role))
            {
                ModelState.AddModelError("CombinationExists", "Användaren har redan denna roll");
                return View();
            }
            return View("Success", addrole);
        }

        public async Task<IActionResult> EditRoles(string id)
        {
            return View();
        }

        public async Task<IActionResult> EditUserRoles(string id)
        {
            var viewModel = new EditUserRolesViewModel();

            viewModel.AllRoles = await _auth.GetRolesAsSelectListItems();
            viewModel.User = await _auth.GetUserById(id);
            viewModel.SelectedRoles = await _auth.GetUsersRole(viewModel.User);

            viewModel.Id = viewModel.User.Id;
            viewModel.Email = viewModel.User.Email;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles(EditUserRolesViewModel viewModel)
        {
            var user = await _auth.GetUserById(viewModel.Id);

            IEnumerable<string> oldRoles = _auth.GetUsersRole(user).Result;

            var rolesToDelete = oldRoles.Where(oldRoleName => viewModel.SelectedRoles.All(newRoleName => newRoleName != oldRoleName));

            await _auth.DeleteRole(user, rolesToDelete);
            //rolesToDelete.ToList().ForEach(x => _auth.DeleteRole(x))

            viewModel.SelectedRoles.ToList().ForEach( x =>  _ = _auth.AddRoleToUser(viewModel.Id, x).Result);

            

            //    ViewModel.AllRoles = await _auth.GetRolesAsSelectListItems();
            //    ViewModel.User = await _auth.GetUserById(id);
            //    ViewModel.SelectedRoles = await _auth.GetUsersRole(ViewModel.User);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UploadImage([FromBody]string image)
        {
            System.Security.Claims.ClaimsPrincipal u = User;
            string fileNameWitPath = _env.WebRootPath + @"/images/" + _auth.GetUserId(User) + ".png";
            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(image);
                    bw.Write(data);
                    bw.Close();
                }
            }
            return Ok();
        }
    }
}
