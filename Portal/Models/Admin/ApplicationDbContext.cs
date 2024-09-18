using Microsoft.EntityFrameworkCore;

namespace Portal.Models.Admin
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Menu> Menus { get; set; }
      
        // public DbSet<Personel> Personeller { get; set; }
        public DbSet<Birim> Birimler { get; set; }
        public DbSet<Mudurluk> Mudurlukler { get; set; }
        public DbSet<Sube> Subeler { get; set; }
        public DbSet<KanGrubu> KanGrubus { get; set; }


         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

           

 // User - Sube Relationship
    modelBuilder.Entity<User>()
        .HasOne(u => u.Sube)
        .WithMany(s => s.Users)
        .HasForeignKey(u => u.SubeId)
        .OnDelete(DeleteBehavior.SetNull);

    // Birim - Mudurluk Relationship
    modelBuilder.Entity<Birim>()
        .HasMany(b => b.Mudurlukler)
        .WithOne(m => m.Birim)
        .HasForeignKey(m => m.BirimId)
        .OnDelete(DeleteBehavior.Cascade);

    // Mudurluk - Sube Relationship
    modelBuilder.Entity<Mudurluk>()
        .HasMany(m => m.Subeler)
        .WithOne(s => s.Mudurluk)
        .HasForeignKey(s => s.MudurlukId)
        .OnDelete(DeleteBehavior.Cascade);
       // User ve KanGrubu arasındaki Foreign Key ilişkisi
        modelBuilder.Entity<User>()
            .HasOne(u => u.KanGrubu)
            .WithMany(k => k.Users)
            .HasForeignKey(u => u.KanGrubuId)
            .OnDelete(DeleteBehavior.SetNull); // İsteğe bağlı: Eğer KanGrubu silinirse, User kaydındaki KanGrubuId null olur.

        // KanGrubu tablosundaki Ad alanı için zorunluluk tanımlama
        modelBuilder.Entity<KanGrubu>()
            .Property(k => k.Ad)
            .IsRequired()
            .HasMaxLength(50); // İsteğe bağlı: Ad alanının uzunluğunu sınırlayabilirsin.
        }
    }
}
 }

   
