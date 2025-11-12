using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Data.Entities;

namespace ProductStore.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
