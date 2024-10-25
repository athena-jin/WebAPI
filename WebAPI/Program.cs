using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.OData;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Options;
using Microsoft.OData.ModelBuilder;
using WebAPI.Controllers;
using WebAPI.Data;
namespace WebAPI
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddControllers();
            var dbPath = Path.Combine(AppContext.BaseDirectory, "db_file", "machine.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
            builder.Services.AddDbContext<CustomDbContext>(opt =>
                opt.UseSqlite($"Data Source={dbPath}"));

            // ÅäÖÃ OData
            SetOdata(builder);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            var app = builder.Build();

            // Automatically apply migrations
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CustomDbContext>();
                dbContext.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        protected static void SetOdata(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers().AddOData(options =>
            {

                options.Select().Expand().Filter().OrderBy().Count().SetMaxTop(100);
                var modelBuilder = new ODataConventionModelBuilder();
                modelBuilder.EntitySet<Machine>("Machines");
                modelBuilder.EntitySet<Actor>("Actors");
                modelBuilder.EntitySet<Video>("Videos");
                //modelBuilder.EntitySet<TodoItem>("Todos");
                options.AddRouteComponents("odata", modelBuilder.GetEdmModel());
            });
        }
    }
}
