using noCarbon.Core.Dtos;

namespace noCarbon.Services.Historics;

public interface IHistoricService
{
    Task<IList<HistoricDto>> GetAll(Guid? CustomerId, int ? CategoryId = null, int? ActionId = null);
    Task<HistoricDto> GetById(int id);
    Task Add(AddHistoricDto dto);
    Task Delete(int id);
    Task<GetLeaderboardDto> GetLeaderboard(Guid? CustomerId);
    Task<IList<GetWeeklyTrendDto>> GetMyWeeklyTrend(Guid CustomerId);
    Task<IList<GetYearlyTrendDto>> GetYearlyTrend(Guid CustomerId);
}
