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
        }
    }
    public enum MachineStatus
    {
        Init =0,
        Running = 1,
        Closed = 2,
    }
    public class Machine
    {

        [Key]
        public Guid Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        public string Address { get; set; }
        public uint Port { get; set; }
        public MachineStatus Status { get; set; }
    }
    public class TodoItem
    {
        [Key]
        public Guid Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
    public class Video
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ActorId { get; set; }
        [ForeignKey(nameof(ActorId))]
        public Actor Actor { get; set; }
    }
    public class Actor
    {
        [Key]
        public Guid Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        [NotMapped]
        public int Age
        {
            get
            {
                return DateTime.UtcNow.Year - BirthTime.Year;
            }
        }
        public DateTime BirthTime { get; set; }
    }
}
