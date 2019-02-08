using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC02.Models;
using MVC02.Models.Entities;

namespace MVC02.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductsRoles>().HasKey(sc => new { sc.RoleId, sc.ProductId });
            base.OnModelCreating(modelBuilder);
        }

        
        public DbSet<MVC02.Models.Product> Product { get; set; }
        public DbSet<MVC02.Models.Entities.Category> Category { get; set; }
        public DbSet<MVC02.Models.Contact> Contact { get; set; }
    }
}
