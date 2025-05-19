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
    public class User : NameEntity
    {
        public string Password { get; set; }
    }
    #endregion
}
