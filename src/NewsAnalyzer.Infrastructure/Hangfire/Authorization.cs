using Hangfire.Dashboard;

namespace NewsAnalyzer.Infrastructure.Hangfire;

public class Authorization
{
    public class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}