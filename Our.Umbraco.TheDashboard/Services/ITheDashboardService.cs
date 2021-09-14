using System.Collections.Generic;
using Our.Umbraco.TheDashboard.Models.Dtos;

namespace Our.Umbraco.TheDashboard.Services
{
    public interface ITheDashboardService
    {
        List<LogEntryDto> GetEntries();

        List<LogEntryDto> GetUnpublished();

        List<LogEntryDto> GetPending();
    }
}