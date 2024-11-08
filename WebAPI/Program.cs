using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using WebAPI.Controllers;
using WebAPI.Data;
using WebAPI.Hubs;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ���� SignalR ����
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<OpcUaService>();

            // �������ݿ�
            var dbPath = Path.Combine(AppContext.BaseDirectory, "db_file", "machine.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
            builder.Services.AddDbContext<CustomDbContext>(opt =>
                opt.UseSqlite($"Data Source={dbPath}"));

            // ���� OData
            ConfigureOData(builder);

            // ��� API �ĵ�������
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            var app = builder.Build();

            // �Զ�Ӧ�����ݿ�Ǩ��
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CustomDbContext>();
                dbContext.Database.Migrate();
            }

            // ���� HTTP ������ܵ�
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseRouting();

            // �����ս��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<OpcUaHub>("/opcuahub");  // ���� Hub ·��
            });

            app.Run();
        }

        private static void ConfigureOData(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers().AddOData(options =>
            {
                options.Select().Expand().Filter().OrderBy().Count().SetMaxTop(100);
                var modelBuilder = new ODataConventionModelBuilder();
                modelBuilder.EntitySet<Machine>("Machines");
                modelBuilder.EntitySet<Actor>("Actors");
                modelBuilder.EntitySet<Video>("Videos");
                options.AddRouteComponents("odata", modelBuilder.GetEdmModel());
            });
        }
    }
}
