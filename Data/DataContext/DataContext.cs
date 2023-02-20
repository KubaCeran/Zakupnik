using Microsoft.EntityFrameworkCore;
using Zakupnik.Data.Entities;
using Zakupnik.Data.Mappings;

namespace Zakupnik.Data.DataContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderProduct> OrdersToProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UsersMapping());
            builder.ApplyConfiguration(new CategoriesMapping());
            builder.ApplyConfiguration(new OrdersMapping());
            builder.ApplyConfiguration(new ProductsMapping());
            builder.ApplyConfiguration(new OrdersToProductsMapping());





            base.OnModelCreating(builder);
        }
    }

    

}
