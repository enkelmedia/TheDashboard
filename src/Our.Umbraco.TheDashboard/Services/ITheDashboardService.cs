using Our.Umbraco.TheDashboard.Models.Dtos;

namespace Our.Umbraco.TheDashboard.Services;

public interface ITheDashboardService
{
    List<LogEntryDto> GetEntries();

    List<LogEntryDto> GetPending();
}
