using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Data.Entities;

namespace ProductStore.Web.Data.Seeders
{
    public class PermissionSeeder
    {
        private readonly DataContext _context;

        public PermissionSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions = [.. Product(), .. Category()];

            foreach (Permission permission in permissions)
            {
                bool exists = await _context.Permission.AnyAsync(p => p.Name == permission.Name);

                if (!exists)
                {
                    await _context.Permission.AddAsync(permission);
                }
            }

            await _context.SaveChangesAsync();
        }

        private List<Permission> Product()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Name = "showProduct", Description = "Ver Productos", Module = "Productos"},
                new Permission { Id = Guid.NewGuid(), Name = "createProduct", Description = "Crear Productos", Module = "Productos"},
                new Permission { Id = Guid.NewGuid(), Name = "updateProduct", Description = "Editar Productos", Module = "Productos"},
                new Permission { Id = Guid.NewGuid(), Name = "deleteProduct", Description = "Eliminar Productos", Module = "Productos"},
            };
        }

        private List<Permission> Category()
        {
            return new List<Permission>
            {
                new Permission { Name = "showCategories", Description = "Ver Categorías", Module = "Categorías"},
                new Permission { Name = "createCategories", Description = "Crear Categorías", Module = "Categorías"},
                new Permission { Name = "updateCategories", Description = "Editar Categorías", Module = "Categorías"},
                new Permission { Name = "deleteCategories", Description = "Eliminar Categorías", Module = "Categorías"},
            };
        }
    }
}
