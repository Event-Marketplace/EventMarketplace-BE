using EventMarketplace.Domain.Repositories;
using EventMarketplace.Infrastructure.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

namespace EventMarketplace.Infrastructure.Crons;

public class SetUnActiveEventsCronJob(IEventRepository eventRepository, ILogger<SetUnActiveEventsCronJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await eventRepository.SetUnActiveByDate();
            logger.LogInformation($"Poprawnie wykonano ustawienie flagi - IsActive:  {DateTime.Now}");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Błąd podczas zmiany stanu wydarzenia");
            throw;
        }
    }
}