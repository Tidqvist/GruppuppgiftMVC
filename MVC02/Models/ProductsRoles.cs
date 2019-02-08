using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC02.Models
{
    public class ProductsRoles
    {
        public string RoleId { get; set; }
        public AppRole AppRole { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
