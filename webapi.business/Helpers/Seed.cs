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
                    new Role{Name="Voluntario"}
                };

                foreach (var item in roles)
                {
                    roleManager.CreateAsync(item).Wait();
                }

                var user = SetUser();
                var userCreate = userManager.CreateAsync(user, "*Admin123").Result;

                if (userCreate.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("admin").Result;
                    userManager.AddToRoleAsync(admin, "SuperAdministrador").Wait();
                    userManager.AddToRoleAsync(admin, "Administrador").Wait();
                    userManager.AddToRoleAsync(admin, "Voluntario").Wait();
                }
            }
        }
        private static User SetUser()
        {
            var persona = new Persona {
                Nombres = "Carlos",
                Apellidos = "Ramirez",
                Genero = "Masculino",
                Domicilio = "Barrio Senac",
                FechaNacimiento = new DateTime(1992, 9, 24),
                Telefono = "76980199"
            };
            var user = new User
            {
                Persona = persona,
                Estado = "Activo",
                UserName = "admin",
                Email = "miranda76575@gmail.com",
                EmailConfirmed = true,
                FechaCreacion = DateTime.Now
            };
            return user;
        }
    }
}
