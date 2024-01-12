using BlazorAppCatalogo.Server.Context;
using BlazorAppCatalogo.Server.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppCatalogo;

internal static class HostingExtensions
{
  public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
  {
    // Add services to the container.
    var configuration = builder.Configuration;


    builder.Services.AddControllers();
    builder.Services.AddRazorPages();

    builder.Services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("AppCatalogo")
        ));

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    return builder.Build();
  }

  public static WebApplication ConfigurePipeline(this WebApplication app)
  {
    app.UseHttpsRedirection();
    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseAuthorization();

    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");

    app.Run();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
      app.UseWebAssemblyDebugging();
    }
    return app;
  }
}