using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Data.Entities;
using static System.Collections.Specialized.BitVector32;

namespace ProductStore.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<ProductStoreRole> ProductStoreRole { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureIndexes(builder);

            ConfigureKeys(builder);

            base.OnModelCreating(builder);
        }

        private void ConfigureKeys(ModelBuilder builder)
        {
            // Role Permissions
            builder.Entity<RolePermission>().HasKey(rp => new { rp.PermissionId, rp.ProductStoreRoleId });

            builder.Entity<RolePermission>().HasOne(rp => rp.ProductStoreRole)
                                            .WithMany(r => r.RolePermission)
                                            .HasForeignKey(rp => rp.ProductStoreRoleId);

            builder.Entity<RolePermission>().HasOne(rp => rp.Permission)
                                            .WithMany(p => p.RolePermissions)
                                            .HasForeignKey(rp => rp.PermissionId);
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            // Roles
            builder.Entity<ProductStoreRole>().HasIndex(psr => psr.Name)
                                             .IsUnique();

            // Sections
            builder.Entity<Category>().HasIndex(c => c.Name)
                                     .IsUnique();

            // User
            builder.Entity<User>().HasIndex(u => u.Document)
                                  .IsUnique();

            // Permission
            builder.Entity<Permission>().HasIndex(p => p.Name)
                                        .IsUnique();
        }

    }
}
