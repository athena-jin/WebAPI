using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
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

            // 配置 SignalR 服务
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<OpcUaService>();

            // 配置数据库
            var dbPath = Path.Combine(AppContext.BaseDirectory, "db_file", "machine.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
            builder.Services.AddDbContext<CustomDbContext>(opt =>
                opt.UseSqlite($"Data Source={dbPath}"));

            // 配置 OData
            ConfigureOData(builder);

            // 添加 API 文档生成器
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());


                // 添加Token验证配置
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "在下方输入令牌：Bearer {your token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // 自动应用数据库迁移
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CustomDbContext>();
                dbContext.Database.Migrate();
            }

            // 配置 HTTP 请求处理管道
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            // 配置终结点
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<OpcUaHub>("/opcuahub");  // 配置 Hub 路径
            });

            app.UseAuthorization();

            app.Run();
        }

        private static void ConfigureOData(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers().AddOData(options =>
            {
                options.Select().Expand().Filter().OrderBy().Count().SetMaxTop(100);
                var modelBuilder = new ODataConventionModelBuilder();
                modelBuilder.EntitySet<Machine>("Machines");
                modelBuilder.EntitySet<User>("Users");
                options.AddRouteComponents("odata", modelBuilder.GetEdmModel());
            });
        }
    }
}
