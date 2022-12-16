using noCarbon.Core.Domain;
using noCarbon.Core.Dtos;
using noCarbon.Core.Exceptions;
using noCarbon.Data;
using noCarbon.Data.Configurations;
using noCarbon.Services.Action;
using noCarbon.Services.Categories;

namespace noCarbon.Services.Historics;

public partial class HistoricService : IHistoricService
{
    #region Fields
    private readonly IRepository<Historic> _HistoricRepository;
    private readonly IActionsService _actionsService;
    private readonly ICategoryService _categoryService;
    private readonly AppDbContext _appDbContext;
    #endregion
    #region Ctor
    public HistoricService(IRepository<Historic> HistoricRepository,
        IActionsService actionsService,
        ICategoryService categoryService,
        AppDbContext appDbContext)
    {
        this._HistoricRepository = HistoricRepository;
        this._actionsService = actionsService;
        this._appDbContext = appDbContext;
        this._categoryService = categoryService;
    }
    #endregion
    #region Methods

    public async Task<IList<HistoricDto>> GetAll(Guid? CustomerId, int? CategoryId = null, int? ActionId = null)
    {
        var result = from p in _appDbContext.GetHistoric(CustomerId, CategoryId, ActionId)
                     orderby p.OperationDate, p.OperationTime
                     select new HistoricDto
                     {
                         CustomerId = p.CustomerId,
                         ActionName = p.ActionName,
                         CategoryName = p.CategoryName,
                         OperationDate = p.OperationDate,
                         OperationTime=p.OperationTime,
                         Points = p.Points,
                         ReducedCarb = p.ReducedCarb
                     };
        return await result.ToListAsync();
    }
    public async Task<HistoricDto> GetById(int id)
    {
        var historic = _HistoricRepository.GetById(id);
        var action = await _actionsService.GetById(historic.ActionId);
        return new HistoricDto()
        {
            CustomerId = historic.CustomerId,
            ActionName = action.Name,
            CategoryName = action.CategoryName,
            OperationDate = historic.OperationDate,
            OperationTime = historic.OperationTime,
            Points = historic.Points,
            ReducedCarb = historic.ReducedCarb
        };
    }
    public async Task Add(AddHistoricDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        var action = await _actionsService.GetById(dto.ActionId);
        if (action == null)
            throw new EntityNotFoundException(string.Format("cannot find an entity {0} with the identifier {1} ", typeof(Actions), dto.ActionId));
        var category = await _categoryService.GetById(dto.CategoryId);
        if (category == null)
            throw new EntityNotFoundException(string.Format("cannot find an entity {0} with the identifier {1} ", typeof(Category), dto.CategoryId));
        _HistoricRepository.Insert(new Historic
        {
            CategoryId = dto.CategoryId,
            CustomerId = dto.CustomerId,
            ActionId = dto.ActionId,
            Points = action.Points,
            ReducedCarb = action.ReducedCarb
        });
    }

    public async Task Delete(int id)
    {
        if (id == 0)
            throw new ArgumentNullException(nameof(id));
        var Historic = _HistoricRepository.GetById(id);
        await _HistoricRepository.DeleteAsync(Historic);
    }
    public async Task<GetLeaderboardDto> GetLeaderboard(Guid? CustomerId)
    {
        var query = from p in await _appDbContext.GetLeaderboard()
                    orderby p.TotalImpact
                    select p;

        var result = new GetLeaderboardDto
        {
            Leaderboards = await query.ToListAsync(),
        };
        for (int i = 0; i < result.Leaderboards.Count; i++)
        {
            result.Leaderboards.ElementAt(i).Classement = i;
            if (CustomerId.HasValue && CustomerId == result.Leaderboards.ElementAt(i).CustomerId)
            {
                result.CurrentCustomer = result.Leaderboards.ElementAt(i);
                result.Leaderboards.RemoveAt(i);
            }

        }
        return result;
    }
    public async Task<IList<GetWeeklyTrendDto>> GetMyWeeklyTrend(Guid CustomerId)
    {
        var query = from p in await _appDbContext.GetMyWeeklyTrend(CustomerId)
                    orderby p.TotalImpact
                    select new GetWeeklyTrendDto
                    {
                        DayOfTheWeek = p.DayOfTheWeek,
                        TotalImpact = p.TotalImpact
                    };

        return await query.ToListAsync();
    }
    public async Task<IList<GetYearlyTrendDto>> GetYearlyTrend(Guid CustomerId)
    {
        var query = from p in await _appDbContext.GetYearlyTrend(CustomerId)
                    orderby p.TotalImpact
                    select new GetYearlyTrendDto
                    {
                        Year = p.Year,
                        TotalImpact = p.TotalImpact,
                        Consomer = p.Consomer
                    };

        return await query.ToListAsync();
    }
    #endregion

}
