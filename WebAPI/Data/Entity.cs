using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebAPI.Data
{

    #region Model Entity
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
    public class NameEntity : Entity
    {
        [NotNull]
        public string Name { get; set; }
    }

    public enum MachineStatus
    {
        Init = 0,
        Running = 1,
        Closed = 2,
    }
    public enum MachineConnectorType
    {
        TCP = 0,
        Sharp7 = 1,
        OPC_UA = 2,
    }
    public partial class Machine : NameEntity
    {
        public string Address { get; set; }
        public uint Port { get; set; }
        public MachineStatus Status { get; set; }
        public MachineConnectorType ConnectorType { get; set; } = MachineConnectorType.TCP;
        [NotMapped]
        public string? Details { get; set; }
    }
    /// <summary>
    /// 可能需要jsonx的序列化规则
    /// </summary>
    public class MachineDetails
    {
    }
    public class Video : NameEntity
    {
        public Guid? ActorId { get; set; }
        [ForeignKey(nameof(ActorId))]
        public Actor Actor { get; set; }
    }
    public class Actor : NameEntity
    {
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
    #endregion
}
