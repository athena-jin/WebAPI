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
        public DbSet<TodoItem> TodoItems { get; set; } = null!;
    }
    public class Machine
    {

        [Key]
        public Guid Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        public string Address { get; set; }
        public uint Port { get; set; }
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
