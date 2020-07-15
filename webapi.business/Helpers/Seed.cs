using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using webapi.core.Models;

namespace webapi.business.Helpers
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                //var userData = System.IO.File.ReadAllText("Data/FirstSeed.json");
                //var users = JsonConvert.DeserializeObject<List<User>>(userData);

                //create roles
                var roles = new List<Role>
                {
                    new Role{Name="Admin"},
                    new Role{Name="Voluntario"},
                    new Role{Name="Adoptante"},
                };

                foreach (var item in roles)
                {
                    roleManager.CreateAsync(item).Wait();
                }

                var user = setAdminUser();
                var result = userManager.CreateAsync(user, "admin").Result;
                //userManager.AddToRoleAsync(user,"Admin");

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                    userManager.AddToRoleAsync(admin, "Voluntario").Wait();
                }
            }
        }
        private static User setAdminUser()
        {
            var u = new User();
            u.Nombres = "Admin";
            u.Apellidos = "Admin";
            u.Estado = "Activo";
            u.UserName = "admin";

            return u;
        }
    }
}
