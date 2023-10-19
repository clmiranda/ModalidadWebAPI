using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public virtual DbSet<SolicitudAdopcion> SolicitudAdopcion { get; set; }
        public virtual DbSet<AdopcionRechazada> AdopcionRechazada { get; set; }
        public virtual DbSet<AdopcionCancelada> AdopcionCancelada { get; set; }
        public virtual DbSet<Mascota> Mascota { get; set; }
        public virtual DbSet<ReporteSeguimiento> ReporteSeguimiento { get; set; }
        public virtual DbSet<ReporteTratamiento> ReporteTratamiento { get; set; }
        public virtual DbSet<Seguimiento> Seguimiento { get; set; }
        public virtual DbSet<Denuncia> Denuncia { get; set; }
        public virtual DbSet<Foto> Foto { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<ContratoAdopcion> ContratoAdopcion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=DatabaseSpat;Username=postgres;Password=qubyx_*7");
//            }
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
                entity.Property(e => e.Estado).HasMaxLength(50);
            });

            modelBuilder.Entity<Persona>()
                        .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                        .HasOne(x => x.Persona)
                        .WithOne(x => x.User)
                        .HasForeignKey<Persona>(x => x.Id);


            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityRoleClaim<string>>();

            modelBuilder.Entity<User>()
                .Ignore(x => x.AccessFailedCount)
                .Ignore(x => x.LockoutEnabled)
                .Ignore(x => x.TwoFactorEnabled)
                .Ignore(x => x.LockoutEnd)
                .Ignore(x => x.PhoneNumberConfirmed)
                .Ignore(x => x.PhoneNumber);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}