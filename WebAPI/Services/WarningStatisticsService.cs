using System;
using WebAPI.Data;

namespace WebAPI.Services;

/// <summary>
/// 描述：定时统计预警信息服务
/// 公司：Yells
/// 作者：金杨阳
/// 创建日期：2025/4/7 16:10:36
/// 版本：v1.0
/// =============================================================
/// </summary>
public class WarningStatisticsService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public WarningStatisticsService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //凌晨12点后执行
            var now = DateTime.Now;
            var nextRunTime = now.Date.AddDays(1); // Next run time is at 12:00 AM next day
            var delay = nextRunTime - now;

            Console.WriteLine($"WarningStatisticsService will run at {nextRunTime}");

            await Task.Delay(delay, stoppingToken);

            if (!stoppingToken.IsCancellationRequested)
            {
                await DoWork(stoppingToken);
            }
        }
    }

    /// <summary>
    /// 处理历史记录，否则将可能会有没有统计到的数据
    /// </summary>
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        //while (!stoppingToken.cancellationToken)
        Console.WriteLine("开始整理历史数据");
        var yesterday = DateTime.Now.Date.AddDays(-1);

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CustomDbContext>();

        //已统计的日期
        var dates = context.WarningRecordDetails.Where(_ => _.Date <= yesterday).Select(_ => _.Date).Distinct();

        Console.WriteLine($"已统计历史数据{dates.Count()}条");

        dates = dates.Distinct();

        var records = context.WarningRecords.Where(_ => _.Time <= yesterday &&! dates.Contains(_.Time.Date)).ToList();

        if (records != null && records.Count() != 0)
        {

            Console.WriteLine($"未统计历史数据{records.Count()}条");
            foreach (var group in records.GroupBy(_ => _.Time.Date))
            {
                //按照缺陷等级和设备名称分组
                var groupedWarnings = records
                    .GroupBy(w => new
                    {
                        w.WarningLevel,
                        w.MachineId,
                    })
                    .Select(warningGroup => new WarningRecordDetails
                    {
                        Date = group.Key,
                        WarningLevel = warningGroup.Key.WarningLevel,
                        MachineId = warningGroup.Key.MachineId,
                        TotalCount = warningGroup.Count()
                    });

                if (groupedWarnings.Any())
                {
                    context.WarningRecordDetails.AddRange(groupedWarnings);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"缺陷记录，日期：{group.Key:yyyy-MM-dd} 已统计");
                }
                else
                {

                    Console.WriteLine($"日期：{group.Key:yyyy-MM-dd} 无缺陷记录");
                }
            }
        }

        Console.WriteLine("整理历史数据结束");

    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        try
        {
            var yesterday = DateTime.Now.Date.AddDays(-1);

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CustomDbContext>();

            //如果昨天的数据已经统计则跳过此步骤
            if (context.WarningRecordDetails.Any(_=>_.Date == yesterday))
            {
                return;
            }

            var records = context.WarningRecords.Where(_ => _.Time >= yesterday && _.Time < DateTime.Now.Date);

            if (records != null && records.Count() != 0)
            {

                Console.WriteLine($"未统计历史数据{records.Count()}条");
                foreach (var group in records.GroupBy(_ => _.Time.Date))
                {
                    //按照缺陷等级和设备名称分组
                    var groupedWarnings = records
                        .GroupBy(w => new
                        {
                            w.WarningLevel,
                            w.Machine,
                        })
                        .Select(warningGroup => new WarningRecordDetails
                        {
                            Date = group.Key,
                            WarningLevel = warningGroup.Key.WarningLevel,
                            Machine = warningGroup.Key.Machine,
                            TotalCount = warningGroup.Count()
                        });

                    if (groupedWarnings.Any())
                    {
                        await context.WarningRecordDetails.AddRangeAsync(groupedWarnings.ToArray());
                        await context.SaveChangesAsync();
                        Console.WriteLine($"缺陷记录，日期：{group.Key:yyyy-MM-dd} 已统计");
                    }
                    else
                    {

                        Console.WriteLine($"日期：{group.Key:yyyy-MM-dd} 无缺陷记录");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while executing WarningStatisticsService");
        }
    }
}

