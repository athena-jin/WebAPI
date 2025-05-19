using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebAPI.Data;

namespace WebAPI.Services;

/// <summary>
/// 描述：创建测试数据预警信息服务，每天会产生只是数十条数据
/// </summary>
public class TestDataCreatorService : BackgroundService
{
    private DateTime CurrentDay = DateTime.Now.AddMonths(-1);

    private readonly IServiceProvider _serviceProvider;
    public TestDataCreatorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //凌晨12点后执行
            var now = DateTime.Now;
            //5分钟执行一次
            var delay = TimeSpan.FromMinutes(1);

            await Task.Delay(delay, stoppingToken);

            if (!stoppingToken.IsCancellationRequested)
            {
                await DoWork(stoppingToken);
            }
        }
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CustomDbContext>();
        var machines = context.Machines;
        var machinesCount = machines.Count();
        Random random = new Random();

        List<WarningRecord> records = new List<WarningRecord>();
        //DetectType 1-7
        for (int index = 1; index < 7; index++)
        {
            WarningRecord details = new WarningRecord()
            {
                Time = DateTime.Now,
                WarningLevel = (WarningLevel)(random.Next(0, 2)),
                Machine = machines.ElementAt(random.Next(0, machinesCount - 1))
            };
            CurrentDay = CurrentDay.AddDays(1);
            records.Add(details);
        }
        await context.WarningRecords.AddRangeAsync(records.ToArray());
        await context.SaveChangesAsync();
    }
}


