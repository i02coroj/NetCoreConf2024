using Microsoft.EntityFrameworkCore;
using NetConf2024Search.Seeder.Model;

namespace NetConf2024Search.Seeder;

public class SearchDbContext(DbContextOptions<SearchDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
}