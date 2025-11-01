using Hangfire;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Common.Services;
using NewsAnalyzer.Infrastructure.External.Fmp;
using NewsAnalyzer.Infrastructure.Hangfire;

namespace NewsAnalyzer.Infrastructure.Common;

public sealed class BackgroundJobScheduler : IBackgroundJobScheduler
{
    private readonly IRecurringJobManager _recurringJobManager;
    
    public BackgroundJobScheduler(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }
    
    public void ScheduleAll()
    {
        // Schedule news import job to run hourly
        _recurringJobManager.AddOrUpdate<NewsImportService>(
            ScheduledJob.NewsImportJob,
            job => job.RunImportAsync(),
            Cron.Hourly);
    }
}