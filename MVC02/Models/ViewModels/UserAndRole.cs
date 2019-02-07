using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC02.Models.ViewModels
{
    public class UserAndRoles
    {
        public IdentityUser User { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
