using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using webapi.core.Models;

namespace webapi.data
{
    public partial class BDSpatContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
        UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public BDSpatContext()
        {
        }

        public BDSpatContext(DbContextOptions<BDSpatContext> options)
            : base(options)
        {
        }
        public virtual DbSet<ContratoAdopcion> ContratoAdopcion { get; set; }
        public virtual DbSet<ContratoRechazo> ContratoRechazo { get; set; }
        public virtual DbSet<Mascota> Mascota { get; set; }
        public virtual DbSet<ReporteSeguimiento> ReporteSeguimiento { get; set; }
        public virtual DbSet<ReporteTratamiento> ReporteTratamiento { get; set; }
        public virtual DbSet<Seguimiento> Seguimiento { get; set; }
        public virtual DbSet<Denuncia> Denuncia { get; set; }
        public virtual DbSet<Foto> Foto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-O7T5KFV\\SQLEXPRESS;Initial Catalog=DatabaseSpat;Integrated Security=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserName).HasMaxLength(50);
                entity.HasIndex(e => e.UserName).IsUnique();


                //entity.Property(e => e.Nombres).HasMaxLength(200);
                //entity.Property(e => e.Apellidos).HasMaxLength(200);
                //entity.Property(e => e.Domicilio).HasMaxLength(3000);
                //entity.Property(e => e.NumeroCelular).HasMaxLength(30);
                entity.Property(e => e.Estado).HasMaxLength(50);
                //entity.Property(e => e.Sexo).HasMaxLength(50);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
