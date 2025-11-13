using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Core;
using ProductStore.Web.Data.Entities;
using ProductStore.Web.Services.Abstractions;

namespace ProductStore.Web.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUsersServices _usersServices;
        private const string CONTENT_MANAGER_ROLE_NAME = "Gestor de contenido";
        private const string BASIC_ROLE_NAME = "Basic";

        public UserRolesSeeder(DataContext context, IUsersServices usersServices)
        {
            _context = context;
            _usersServices = usersServices;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsersAsync();
        }

        private async Task CheckRolesAsync()
        {
            await AdminRoleAsync();
            await BasicRoleAsync();
            await ContentManagerRoleAsync();
        }

        private async Task CheckUsersAsync()
        {
            // Admin
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "kmacamonsalve@gmail.com");

            if (user is null)
            {
                ProductStoreRole adminRole = await _context.ProductStoreRole.FirstOrDefaultAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

                user = new User
                {
                    Email = "manuel@yopmail.com",
                    FirstName = "Manuel",
                    LastName = "Domínguez",
                    PhoneNumber = "3000000000",
                    UserName = "manuel@yopmail.com",
                    Document = "1111",
                    ProductStoreRoleId = adminRole!.Id
                };

                await _usersServices.AddUserAsync(user, "1234");

                string token = (await _usersServices.GenerateConfirmationTokenAsync(user)).Result;
                await _usersServices.ConfirmUserAsync(user, token);
            }

            // Content namager
            user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "lilianahr091@hotmail.com");

            if (user is null)
            {
                ProductStoreRole contentManagerRole = await _context.ProductStoreRole.FirstOrDefaultAsync(r => r.Name == CONTENT_MANAGER_ROLE_NAME);

                user = new User
                {
                    Email = "anad@yopmail.com",
                    FirstName = "Ana",
                    LastName = "Doe",
                    PhoneNumber = "3100000000",
                    UserName = "anad@yopmail.com",
                    Document = "222",
                    ProductStoreRoleId = contentManagerRole!.Id
                };

                await _usersServices.AddUserAsync(user, "1234");

                string token = (await _usersServices.GenerateConfirmationTokenAsync(user)).Result;
                await _usersServices.ConfirmUserAsync(user, token);
            }

            // Basic
            user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == "kevin_vamosverde@hotmail.com");

            if (user is null)
            {
                ProductStoreRole basicRole = await _context.ProductStoreRole.FirstOrDefaultAsync(r => r.Name == BASIC_ROLE_NAME);

                user = new User
                {
                    Email = "jhond@yopmail.com",
                    FirstName = "Jhon",
                    LastName = "Doe",
                    PhoneNumber = "3200000000",
                    UserName = "jhond@yopmail.com",
                    Document = "333",
                    ProductStoreRoleId = basicRole!.Id
                };

                await _usersServices.AddUserAsync(user, "1234");

                string token = (await _usersServices.GenerateConfirmationTokenAsync(user)).Result;
                await _usersServices.ConfirmUserAsync(user, token);
            }
        }

        private async Task AdminRoleAsync()
        {
            bool exists = await _context.ProductStoreRole.AnyAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists)
            {
                ProductStoreRole role = new ProductStoreRole { Id = Guid.NewGuid(), Name = Env.SUPER_ADMIN_ROLE_NAME };
                await _context.ProductStoreRole.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task BasicRoleAsync()
        {
            bool exists = await _context.ProductStoreRole.AnyAsync(r => r.Name == BASIC_ROLE_NAME);

            if (!exists)
            {
                ProductStoreRole role = new ProductStoreRole { Id = Guid.NewGuid(), Name = BASIC_ROLE_NAME };
                await _context.ProductStoreRole.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task ContentManagerRoleAsync()
        {
            bool exists = await _context.ProductStoreRole.AnyAsync(r => r.Name == CONTENT_MANAGER_ROLE_NAME);

            if (!exists)
            {
                ProductStoreRole role = new ProductStoreRole { Id = Guid.NewGuid(), Name = CONTENT_MANAGER_ROLE_NAME };
                await _context.ProductStoreRole.AddAsync(role);

                List<Permission> permissions = await _context.Permission.Where(p => p.Module == "Categorías" || p.Module == "Productos")
                                                                         .ToListAsync();
                foreach (Permission permission in permissions)
                {
                    await _context.RolePermission.AddAsync(new RolePermission { PermissionId = permission.Id, ProductStoreRoleId = role.Id });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
