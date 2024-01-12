using BlazorAppCatalogo.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppCatalogo.Server.Context;
/*
 Gambi para o migrations funcionar Entity Developer, Microsoft SQLite Driver, You need to call SQLitePCL.raw.SetProvider()

    PM> install-package system.data.sqlite
    PM> uninstall-package system.data.sqlite
    PM> install-package microsoft.entityframeworkcore.sqlite
*/



public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }

}
