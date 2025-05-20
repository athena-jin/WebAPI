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
    /// <summary>
    /// 设备异常等级
    /// </summary>
    public enum WarningLevel
    {
        Nomal = 0,
        Warning = 1,
        Error = 2,
    }
    /// <summary>
    /// 设备异常记录
    /// </summary>
    public class WarningRecord : Entity
    {
        public string Message { get; set; } = "";
        public DateTime Time { get; set; }
        public Guid MachineId { get; set; }
        [ForeignKey("MachineId")]
        public Machine Machine { get; set; }
        public WarningLevel WarningLevel { get; set; } = WarningLevel.Nomal;
    }


    /// <summary>
    /// 设备异常记录统计到天
    /// </summary>
    public class WarningRecordDetails : Entity
    {
        public DateTime Date { get; set; }
        public Guid MachineId { get; set; }
        [ForeignKey("MachineId")]
        public Machine Machine { get; set; }
        public WarningLevel WarningLevel { get; set; } = WarningLevel.Nomal;
        public int TotalCount { get; set; }
    }
    #endregion
}
