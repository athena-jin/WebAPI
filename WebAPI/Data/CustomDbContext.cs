using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace WebAPI.Data
{
#nullable disable
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options)
            : base(options)
        {
        }
        public DbSet<Actor> Actors { get; set; } = null!;
        public DbSet<Video> Videos { get; set; } = null!;
        public DbSet<Machine> Machines { get; set; } = null!;

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
                }
            );

            // Seed data for Actors
            modelBuilder.Entity<Actor>().HasData(
                new Actor
                {
                    Id = Guid.NewGuid(),
                    Name = "Actor One",
                    BirthTime = new DateTime(1985, 5, 21)
                },
                new Actor
                {
                    Id = Guid.NewGuid(),
                    Name = "Actor Two",
                    BirthTime = new DateTime(1990, 11, 3)
                }
            );

            // Seed data for Videos
            modelBuilder.Entity<Video>().HasData(
                new Video
                {
                    Id = Guid.NewGuid(),
                    Name = "Video One",
                    ActorId = null // Set to a valid ActorId if necessary
                },
                new Video
                {
                    Id = Guid.NewGuid(),
                    Name = "Video Two",
                    ActorId = null
                }
            );
             modelBuilder.Entity<Machine>().HasData(
                new Machine
                {
                    Id = Guid.NewGuid(),
                    ConnectorType = MachineConnectorType.OPC_UA,
                    Name = "Video One",
                    Port = 4840,
                    Address = "opc.tcp://localhost" // Set to a valid ActorId if necessary
                }
            );
        }
    }
}
