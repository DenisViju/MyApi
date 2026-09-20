using MyApi.Data;
using Microsoft.EntityFrameworkCore;

public class RefreshTokenCleanupService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<RefreshTokenCleanupService> logger;

    private static readonly TimeSpan interval = TimeSpan.FromDays(1);

    public RefreshTokenCleanupService(
        IServiceProvider serviceProvider,
        ILogger<RefreshTokenCleanupService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("RefreshTokenCleanupService pornit.");

        using var timer = new PeriodicTimer(interval);

        try
        {
            await RunCleanupAsync(stoppingToken);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunCleanupAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Aplicatia se opreste normal.
        }
        finally
        {
            logger.LogInformation("RefreshTokenCleanupService oprit.");
        }
    }

    private async Task RunCleanupAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<AplicatieDbContext>();

            var deleted = await context.RefreshTokens
                .Where(rt =>
                    rt.IsRevoked ||
                    rt.ExpiresAt < DateTime.UtcNow)
                .ExecuteDeleteAsync(cancellationToken);

            if (deleted > 0)
            {
                logger.LogInformation(
                    "Refresh token cleanup: {Count} tokenuri sterse.",
                    deleted);
            }
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Eroare la curatarea refresh tokenurilor.");
        }
    }
}