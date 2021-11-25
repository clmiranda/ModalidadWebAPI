using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using webapi.core.Models;

namespace webapi.business.Helpers
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var roles = new List<Role>
                {
                    new Role{Name="SuperAdministrador"},
                    new Role{Name="Administrador"},
                    new Role{Name="Voluntario"},
                    new Role{Name="Moderador"}
                };

                foreach (var item in roles)
                {
                    roleManager.CreateAsync(item).Wait();
                }

                var user = SetAdminUser();
                var result = userManager.CreateAsync(user, "admin").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRoleAsync(admin, "SuperAdministrador").Wait();
                    userManager.AddToRoleAsync(admin, "Administrador").Wait();
                    userManager.AddToRoleAsync(admin, "Voluntario").Wait();
                    //userManager.AddToRoleAsync(admin, "Moderador").Wait();
                }
            }
        }
        private static User SetAdminUser()
        {
            var u = new User
            {
                Nombres = "Admin",
                Apellidos = "Admin",
                Estado = "Activo",
                UserName = "admin",
                Email = "miranda76575@gmail.com",
                EmailConfirmed = true,
                FechaCreacion = DateTime.Now
            };
            return u;
        }
    }
}
