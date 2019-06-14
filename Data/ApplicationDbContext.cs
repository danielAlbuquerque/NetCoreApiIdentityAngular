using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NG_Core_Auth.Models;

namespace NG_Core_Auth.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public DbSet<ProductModel> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Creating Roles for our app
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" },
                new { Id = "3", Name = "Moderator", NormalizedName = "MODERATOR" }
            );
        }

    }
}
