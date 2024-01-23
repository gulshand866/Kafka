using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using System.Collections.Generic;

namespace Order.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Orders> orders{ get; set; }

    }
}
