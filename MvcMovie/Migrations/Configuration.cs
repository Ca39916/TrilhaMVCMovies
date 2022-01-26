namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using MvcMovie.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcMovie.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MvcMovie.Models.ApplicationDbContext";
        }

        //protected override void Seed(MvcMovie.Models.ApplicationDbContext context)

        //  This method will be called after migrating to the latest version.

        //  You can use the DbSet<T>.AddOrUpdate() helper extension method
        //  to avoid creating duplicate seed data.


        protected override void Seed(MvcMovie.Models.ApplicationDbContext context)
        {
            var hasher = new PasswordHasher();
            context.Users.AddOrUpdate(
            u => u.UserName, new ApplicationUser
            {
                UserName = "aa@mvc.br",
                FullName = "Usuario AA",
                PasswordHash = hasher.HashPassword("Pass@word1"),
                SecurityStamp = Guid.NewGuid().ToString()
            });
            new ApplicationUser
            {
                UserName = "admin@mvc.br",
                FullName = "Administrador",
                PasswordHash = hasher.HashPassword("Pass@word1"),
                SecurityStamp = Guid.NewGuid().ToString()
                
            };
            
        }
    }
}
