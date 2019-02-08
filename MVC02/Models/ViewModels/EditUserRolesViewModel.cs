using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC02.Models.ViewModels
{
    public class EditUserRolesViewModel
    {
        public IdentityUser User { get; set; }

        public IEnumerable<SelectListItem> AllRoles { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}