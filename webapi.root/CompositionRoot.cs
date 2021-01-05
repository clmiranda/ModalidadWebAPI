﻿using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Services.Imp;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data;
using webapi.data.Repositories.Imp;
using webapi.data.Repositories.Interf;
using AutoMapper;
using webapi.business.Helpers;

namespace webapi.root
{
    public class CompositionRoot
    {
        public CompositionRoot()
        {
        }

        public static void injectDependencies(IServiceCollection services, IConfiguration configuration)
        {
            /*services.AddDbContext<DatabaseContext>(opts => opts.UseInMemoryDatabase("database"));*/
            services.AddScoped<BDSpatContext>();
            //services.AddDbContext<BDSpatContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<BDSpatContext>(x =>
            {
                x.UseLazyLoadingProxies();
                x.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IAuthorService, AuthorService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IDenunciaService, DenunciaService>();

            //services.AddScoped<ICasoMascotaService, CasoMascotaService>();
            services.AddScoped<IMascotaService, MascotaService>();
            //services.AddScoped<IMascotaRepository, MascotaRepository>();

            services.AddScoped<IContratoAdopcionService, ContratoAdopcionService>();
            //services.AddScoped<IContratoAdopcionRepository, ContratoAdopcionRepository>();

            services.AddScoped<ISeguimientoService, SeguimientoService>();
            services.AddScoped<ISeguimientoRepository, SeguimientoRepository>();

            services.AddScoped<IReporteSeguimientoService, ReporteSeguimientoService>();
            services.AddScoped<IReporteSeguimientoRepository, ReporteSeguimientoRepository>();

            services.AddScoped<IFotoService, FotoService>();
            services.AddScoped<IFotoRepository, FotoRepository>();

            services.AddScoped<IRolUserService, RolUserService>();
            services.AddScoped<IRolUserRepository, RolUserRepository>();

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddCors();
            services.Configure<ConfigurationCloudinary>(configuration.GetSection("CloudinarySettings"));
        }

        public static void otherDependencies(IServiceCollection services, IConfiguration configuration)
        {
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;

                opt.SignIn.RequireConfirmedEmail = true;
                opt.User.RequireUniqueEmail = false;
            }).AddDefaultTokenProviders().AddErrorDescriber<CustomIdentityErrorDescriber>();

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<BDSpatContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication(configureOptions: x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                });
        }
    }
}
