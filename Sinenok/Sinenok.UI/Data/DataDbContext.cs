using Microsoft.EntityFrameworkCore;
using Sinenok.Domain.Entities;

namespace Sinenok.UI.Data
{
    public class DataDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("");
        }

        public DbSet<Gadget> Gadgets { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
