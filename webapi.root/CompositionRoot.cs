using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
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

        public static void InjectDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<BDSpatContext>();
            services.AddDbContext<BDSpatContext>(x =>
            {
                x.UseLazyLoadingProxies();
                x.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IDenunciaService, DenunciaService>();
            services.AddScoped<IMascotaService, MascotaService>();

            services.AddScoped<IAdopcionService, AdopcionService>();

            services.AddScoped<ISeguimientoService, SeguimientoService>();

            services.AddScoped<IReporteSeguimientoService, ReporteSeguimientoService>();

            services.AddScoped<IFotoService, FotoService>();
            services.AddScoped<IGraficaService, GraficaService>();

            services.AddScoped<IRolUserService, RolUserService>();
            services.AddScoped<IRolUserRepository, RolUserRepository>();

            services.AddScoped<IReporteTratamientoService, ReporteTratamientoService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddSingleton<IEmailService, EmailService>();

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
                opt.User.RequireUniqueEmail = true;
            }).AddDefaultTokenProviders().AddErrorDescriber<CustomIdentityErrorDescriber>();
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromHours(1));

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<BDSpatContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication(configureOptions: x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}
