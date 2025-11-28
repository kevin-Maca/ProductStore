using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Data.Entities;
using static System.Collections.Specialized.BitVector32;

namespace ProductStore.Web.Data.Seeders
{
    public class CategorySeeder
    {
        private readonly DataContext _context;

        public CategorySeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Category> categories = new List<Category>()
            {
                new Category { Id = Guid.NewGuid(), Name = "Deportes", Description = "Todo lo relacionado con deportes"},
                new Category { Id = Guid.NewGuid(), Name = "Informática", Description = "Todo lo relacionado con informática"},
                new Category { Id = Guid.NewGuid(), Name = "Infantil", Description = "Juguetes y más"}
            };

            foreach (Category category in categories)
            {
                bool exists = await _context.Category.AnyAsync(c => c.Name == category.Name);

                if (!exists)
                {
                    await _context.Category.AddAsync(category);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
