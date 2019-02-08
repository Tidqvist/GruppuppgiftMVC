using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC02.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole(string roleName) : base(roleName)
        {            
        }
        public AppRole() : base()
        {
        }
        public virtual ICollection<ProductsRoles> ProductRoles { get; set; }
    }
}
