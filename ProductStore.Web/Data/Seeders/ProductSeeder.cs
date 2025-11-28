using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Data;
using ProductStore.Web.Data.Entities;
using static System.Collections.Specialized.BitVector32;

namespace ProductStore.Web.Data.Seeders
{
    public class ProductSeeder
    {
        private readonly DataContext _context;
        public ProductSeeder(DataContext context)
        {
            _context = context;
        }
        public async Task SeedAsync()
        {
            Category category = await _context.Category.FirstOrDefaultAsync();
            List<Product> products = new List<Product>()
            {
                new Product { Id = Guid.NewGuid(), Name = "Martillo", Price = 0, Stock = 0, CategoryId = category.Id },
                new Product { Id = Guid.NewGuid(), Name = "Computador", Price = 0, Stock = 0, CategoryId = category.Id },
                new Product { Id = Guid.NewGuid(), Name = "Tenis", Price = 0, Stock = 0, CategoryId = category.Id},
                new Product { Id = Guid.NewGuid(), Name = "Gomitas", Price = 0, Stock = 0, CategoryId = category.Id },

            };
            foreach (Product product in products)
            {
                bool exists = await _context.Product.AnyAsync(p => p.Name == product.Name);
                if (!exists)
                {
                    await _context.Product.AddAsync(product);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
