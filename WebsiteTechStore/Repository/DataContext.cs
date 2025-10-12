using Microsoft.EntityFrameworkCore;
using WebsiteTechStore.Models;

namespace WebsiteTechStore.Repository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<ProductModel> Products { get; set; }
    }
}
