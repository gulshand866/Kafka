using Microsoft.EntityFrameworkCore;
using Product.API.Models;

namespace Product.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Products> products { get; set; }

    }
}
