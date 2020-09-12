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
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public virtual DbSet<CasoMascota> CasoMascota { get; set; }
        public virtual DbSet<ContratoAdopcion> ContratoAdopcion { get; set; }
        public virtual DbSet<ContratoRechazo> ContratoRechazo { get; set; }
        //public virtual DbSet<DetalleAdopcion> DetalleAdopcion { get; set; }
        //public virtual DbSet<EstadoAdopcion> EstadoAdopcion { get; set; }
        public virtual DbSet<Mascota> Mascota { get; set; }
        public virtual DbSet<Notificacion> Notificacion { get; set; }
        public virtual DbSet<ReporteSeguimiento> ReporteSeguimiento { get; set; }
        public virtual DbSet<Seguimiento> Seguimiento { get; set; }
        //public virtual DbSet<TipoMascota> TipoMascota { get; set; }
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


                entity.Property(e => e.Nombres).HasMaxLength(200);
                entity.Property(e => e.Apellidos).HasMaxLength(200);
                entity.Property(e => e.Domicilio).HasMaxLength(3000);
                entity.Property(e => e.NumeroCelular).HasMaxLength(30);
                entity.Property(e => e.Estado).HasMaxLength(50);
                entity.Property(e => e.Sexo).HasMaxLength(50);
                entity.Property(e => e.UserName).HasMaxLength(50);
            });
            //modelBuilder.Entity<Role>(entity =>
            //{
            //});

            modelBuilder.Entity<CasoMascota>(entity =>
            {
                //entity.HasKey(e => e.IdCasoMascota);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).HasMaxLength(500);
                entity.Property(e => e.Descripcion).HasMaxLength(10000);
                entity.Property(e => e.Estado).HasMaxLength(50);
                //entity.HasOne(d => d.Denuncia)
                //    .WithMany(p => p.CasoMascotas)
                //    .HasForeignKey(d => d.IdDenuncia)
                //    .HasConstraintName("FK_CasoMascota_Denuncia");
            });

            modelBuilder.Entity<ContratoAdopcion>(entity =>
            {
                entity.HasKey(e => e.Id);
                //entity.Property(e => e.NombreCompleto).HasMaxLength(3000);
                //entity.Property(e => e.Domicilio).HasMaxLength(3000);
                //entity.Property(e => e.NumeroCelular).HasMaxLength(50);
                entity.Property(e => e.RazonAdopcion).HasMaxLength(300);
                entity.Property(e => e.Estado).HasMaxLength(50);
                //entity.Property(e => e.Ci).HasMaxLength(50);
                //entity.HasIndex(e => e.Ci).IsUnique();


                //entity.HasOne(d => d.DetalleAdopcion)
                //    .WithMany(p => p.ContratoAdopciones)
                //    .HasForeignKey(d => d.IdDetalleAdopcion)
                //    .HasConstraintName("FK_ContratoAdopcion_DetalleAdopcion");
                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.ContratoAdopciones)
                //    .HasForeignKey(d => d.IdUsuario)
                //    .HasConstraintName("FK_ContratoAdopcion_Usuario");

                //entity.HasOne(d => d.Mascota)
                //    .WithMany(p => p.ContratoAdopciones)
                //    .HasForeignKey(d => d.IdMascota)
                //    .HasConstraintName("FK_ContratoAdopcion_Mascota");

                //entity.HasOne(d => d.EstadoAdopcion)
                //    .WithMany(p => p.ContratoAdopciones)
                //    .HasForeignKey(d => d.IdEstadoAdopcion)
                //    .HasConstraintName("FK_ContratoAdopcion_EstadoAdopcion");
            });


            modelBuilder.Entity<ContratoRechazo>(entity =>
            {
                entity.Property(e => e.RazonRechazo).HasMaxLength(300);
            });

            //modelBuilder.Entity<DetalleAdopcion>(entity =>
            //{
            //    //entity.HasKey(e => e.IdDetalleAdopcion);
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Estado).HasMaxLength(50);
            //    entity.HasOne(d => d.Mascota)
            //        .WithMany(p => p.DetalleAdopciones)
            //        .HasForeignKey(d => d.IdMascota)
            //        .HasConstraintName("FK_DetalleAdopcion_Mascota");
            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.DetalleAdopciones)
            //        .HasForeignKey(d => d.IdUsuario)
            //        .HasConstraintName("FK_DetalleAdopcion_User");
            //});

            //modelBuilder.Entity<EstadoAdopcion>(entity =>
            //{
            //    //entity.HasKey(e => e.IdEstadoAdopcion);
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Nombre).HasMaxLength(100);
            //    entity.Property(e => e.Descripcion).HasMaxLength(10000);
            //});

            modelBuilder.Entity<Mascota>(entity =>
            {
                //entity.HasKey(e => e.IdMascota);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Sexo).HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasMaxLength(3000);
                entity.Property(e => e.Tamaño).HasMaxLength(100);
                entity.Property(e => e.Edad).HasMaxLength(50);
                entity.Property(e => e.EstadoSituacion).HasMaxLength(50);
                //entity.HasOne(d => d.CasoMascota)
                //    .WithMany(p => p.Mascotas)
                //    .HasForeignKey(d => d.IdCasoMascota)
                //    .HasConstraintName("FK_Mascota_CasoMascota");
                //entity.HasOne(d => d.TipoMascota)
                //    .WithMany(p => p.Mascotas)
                //    .HasForeignKey(d => d.IdTipoMascota)
                //    .HasConstraintName("FK_Mascota_TipoMascota");
            });

            modelBuilder.Entity<Notificacion>(entity =>
            {
                //entity.HasKey(e => e.IdNotificacion);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).HasMaxLength(10000);
                entity.Property(e => e.Estado).HasMaxLength(50);
            });

            modelBuilder.Entity<ReporteSeguimiento>(entity =>
            {
                //entity.HasKey(e => e.IdReporteSeguimiento);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).HasMaxLength(10000);
                entity.Property(e => e.Estado).HasMaxLength(50);
                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.ReporteSeguimientos)
                //    .HasForeignKey(d => d.IdUsuario)
                //    .HasConstraintName("FK_ReporteSeguimiento_User");
                //entity.HasOne(d => d.Seguimiento)
                //    .WithMany(p => p.ReporteSeguimientos)
                //    .HasForeignKey(d => d.IdSeguimiento)
                //    .HasConstraintName("FK_ReporteSeguimiento_Seguimiento");
            });

            modelBuilder.Entity<Seguimiento>(entity =>
            {
                //entity.HasKey(e => e.IdSeguimiento);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Estado).HasMaxLength(50);
                //entity.HasOne(d => d.ContratoAdopcion)
                //    .WithMany(p => p.Seguimientos)
                //    .HasForeignKey(d => d.IdContratoAdopcion)
                //    .HasConstraintName("FK_Seguimiento_ContratoAdopcion");
                //entity.HasOne(d => d.ReporteSeguimiento)
                //    .WithMany(p => p.Seguimientos)
                //    .HasForeignKey(d => d.IdReporteSeguimiento)
                //    .HasConstraintName("FK_Seguimiento_ReporteSeguimiento");
            });

            //modelBuilder.Entity<TipoMascota>(entity =>
            //{
            //    //entity.HasKey(e => e.IdTipoMascota);
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Nombre).HasMaxLength(100);
            //    entity.Property(e => e.Estado).HasMaxLength(50);
            //});
            modelBuilder.Entity<Denuncia>(entity =>
            {
                //entity.HasKey(e => e.IdDenuncia);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).HasMaxLength(500);
                entity.Property(e => e.Descripcion).HasMaxLength(10000);
                entity.Property(e => e.Estado).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
