using Microsoft.EntityFrameworkCore;
namespace WebAPI.Data
{
#nullable disable
    /// <summary>
    /// sqlite数据库上下文
    /// 创建迁移脚本命令  Add-Migration {migrationName
    /// </summary>
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Machine> Machines { get; set; } = null!;
        public DbSet<WarningRecord> WarningRecords { get; set; } = null!;

        public DbSet<WarningRecordDetails> WarningRecordDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Machines
            modelBuilder.Entity<Machine>().HasData(
                new Machine
                {
                    Id = Guid.NewGuid(),
                    Name = "Machine A",
                    Address = "192.168.1.10",
                    Port = 8080,
                    Status = MachineStatus.Init
                },
                new Machine
                {
                    Id = Guid.NewGuid(),
                    Name = "Machine B",
                    Address = "192.168.1.11",
                    Port = 8081,
                    Status = MachineStatus.Closed
                },
                new Machine
                {
                    Id = Guid.NewGuid(),
                    ConnectorType = MachineConnectorType.OPC_UA,
                    Name = "Video One",
                    Port = 4840,
                    Address = "opc.tcp://localhost" // Set to a valid ActorId if necessary
                }
            );

            // Seed data for Actors
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "admin",
                    Password = "admin"
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Default",
                    Password = "Default"
                }
            );
        }
    }
}
