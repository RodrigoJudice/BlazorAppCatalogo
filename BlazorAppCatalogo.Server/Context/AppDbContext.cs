//using BlazorAppCatalogo.Server.Models;
using BlazorAppCatalogo.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppCatalogo.Server.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
