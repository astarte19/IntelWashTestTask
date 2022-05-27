using Microsoft.EntityFrameworkCore;
namespace IntelWash.Model
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
           
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesPoint> SalesPoints { get; set; }
        public DbSet<ProvidedProduct> ProvidedProducts { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<SalesId> SalesIds { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleData> SaleDatas { get; set; }
    }
}