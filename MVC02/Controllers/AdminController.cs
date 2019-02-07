using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc02.Services;
using MVC02.Data;
using MVC02.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC02.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _auth;

        public AdminController(ApplicationDbContext context, AuthService auth)
        {
            _context = context;
            _auth = auth;
        }

        public async Task<IActionResult> Index()
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
    }
}
