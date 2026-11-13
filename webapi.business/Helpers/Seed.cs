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
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager, string adminPassword)
        {
            if (!userManager.Users.Any())
            {
                if (string.IsNullOrWhiteSpace(adminPassword))
                    throw new InvalidOperationException("Falta la configuración 'Seed:AdminPassword' necesaria para crear el usuario administrador inicial.");

                var roles = new List<Role>
                {
                    new() {Name="SuperAdministrador"},
                    new() {Name="Administrador"},
                    new() {Name="Voluntario"}
                };

                foreach (var item in roles)
                {
                    roleManager.CreateAsync(item).Wait();
                }

                var user = SetUser();
                var userCreate = userManager.CreateAsync(user, adminPassword).Result;

                if (userCreate.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRoleAsync(admin, "SuperAdministrador").Wait();
                    userManager.AddToRoleAsync(admin, "Administrador").Wait();
                    userManager.AddToRoleAsync(admin, "Voluntario").Wait();
                }
            }
        }
        private static User SetUser()
        {
            var persona = new Persona
            {
                Nombres = "Carlos",
                Apellidos = "Ramirez",
                Genero = "Masculino",
                Domicilio = "Barrio Senac",
                FechaNacimiento = new DateTime(1992, 9, 24),
                Telefono = "76800111"
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
