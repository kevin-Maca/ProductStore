using ProductStore.Web.Services.Abstractions;
using ProductStore.Web.Services.Implementations;

namespace ProductStore.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersServices _usersServices;

        public SeedDb(DataContext context, IUsersServices usersServices)
        {
            _context = context;
            _usersServices = usersServices;
        }

        public async Task SeedAsync()
        {
            await new CategorySeeder(_context).SeedAsync();
            await new PermissionSeeder(_context).SeedAsync();
            await new UserRolesSeeder(_context, _usersServices).SeedAsync();
        }
    }
}
