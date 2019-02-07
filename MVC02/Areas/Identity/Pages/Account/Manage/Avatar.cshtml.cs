﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MVC02.Areas.Identity.Pages.Account.Manage
{
    public class AvatarModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;
        public string message { get; set; }

        public AvatarModel(
            UserManager<IdentityUser> userManager,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            message = "hej";
            return Page();
        }
    }
}