using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mv.Application.Ports.Messaging;
using Mv.Domain.Entities;
using Mv.Domain.Enums;
using Mv.Infrastructure.Persistence;
using Quartz;

namespace Mv.Worker.BackgroundJobs;

[DisallowConcurrentExecution]
public class FallbackOrderCleanupJob(
  AppDbContext dbContext,
  IEventDispatcher eventDispatcher,
  ILogger<FallbackOrderCleanupJob> logger
) : IJob {
  public async Task Execute(IJobExecutionContext context) {
    logger.LogInformation("[+] (Quartz Job) Bắt đầu quét đơn hàng Pending quá hạn 15 phút...");

    try {
      var thresholdTime = DateTime.UtcNow.AddMinutes(-15);

      var expiredOrders = await dbContext.Set<Order>()
        .Include(o => o.Tickets)
        .Where(o =>
          o.Status == OrderStatus.Pending &&
          o.CreatedAt <= thresholdTime &&
          !o.IsDeleted
        ).ToListAsync(context.CancellationToken);

      if (expiredOrders.Count == 0) {
        logger.LogInformation("[-] Không có đơn hàng nào quá hạn cần hủy.");
        return;
      }

      foreach (var order in expiredOrders) {
        order.Cancel();
        logger.LogInformation("Đã chuyển trạng thái đơn hàng {OrderId} sang Canceled.", order.Id);
      }

      await eventDispatcher.DispatchEventsAsync(context.CancellationToken);
      await dbContext.SaveChangesAsync(context.CancellationToken);
      logger.LogInformation("[+] Đã hủy thành công {Count} đơn hàng quá hạn.", expiredOrders.Count);
    } catch (Exception ex) {
      logger.LogError(ex, "[-] Lỗi khi quét và hủy đơn hàng quá hạn.");
    }
  }
}
