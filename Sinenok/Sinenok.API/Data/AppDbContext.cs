using Sinenok.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Sinenok.Domain.Entities;

namespace Sinenok.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Свойства DbSet для доступа к таблицам базы данных
    public DbSet<Category> Categories { get; set; }
    public DbSet<Gadget> Gadgets { get; set; }
}