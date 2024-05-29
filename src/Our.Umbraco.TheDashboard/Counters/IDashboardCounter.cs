using Umbraco.Cms.Infrastructure.Scoping;

namespace Our.Umbraco.TheDashboard.Counters;

public interface IDashboardCounter
{
    /// <summary>
    /// Returns a <see cref="DashboardCounterModel"/> for the current counter.
    /// </summary>
    /// <param name="scope"></param>
    /// <returns></returns>
    DashboardCounterModel GetModel(IScope scope);
}
