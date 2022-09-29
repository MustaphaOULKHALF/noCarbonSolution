using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using noCarbon.API.Infrastracture;
using noCarbon.API.Models;
using noCarbon.Core.Dtos;
using noCarbon.Services.Historics;
using System.Security.Claims;

namespace noCarbon.API.Controllers;

/// <summary>
/// represent historic controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HistoricController : ControllerBase
{
    #region Fields
    private readonly IHistoricService _HistoricService;
    private readonly IMapper _mapper;
    #endregion
    #region Ctor
    /// <summary>
    /// Ctor 
    /// </summary>
    /// <param name="HistoricService">historic service</param>
    /// <param name="mapper">auto mapper</param>
    public HistoricController(IHistoricService HistoricService, IMapper mapper)
    {
        this._HistoricService = HistoricService;
        this._mapper = mapper;
    }
    #endregion
    #region Methods
    /// <summary>
    /// Get all historic
    /// </summary>
    /// <param name="CategoryId">category Identifier</param>
    /// <param name="Action">action Identifier</param>
    /// <returns>list of historic</returns>
    [HttpGet]
    [Route("GetAll")]
    public async Task<Response<IList<HistoricDto>>> GetAll(int? CategoryId, int? Action)
    {
        Guid? customerId = null;
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            var claimCustomerId = identity.FindFirst(ClaimTypes.PrimarySid);
            if (claimCustomerId != null)
                customerId = Guid.Parse(claimCustomerId.Value);
        }
        var result = await _HistoricService.GetAll(customerId, CategoryId, Action);
        return new Response<IList<HistoricDto>>
        {
            Data = result,
            Succeeded = true,
            Message = "Successful operation"
        };
    }
    /// <summary>
    /// Consume an action
    /// </summary>
    /// <param name="input">action information</param>
    /// <returns></returns>
    [HttpPost]
    [Route("Add")]
    public async Task Add(HistoricInput input)
    {
        var dto = _mapper.Map<AddHistoricDto>(input);
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            var claimCustomerId = identity.FindFirst(ClaimTypes.PrimarySid);
            if (claimCustomerId != null)
                dto.CustomerId = Guid.Parse(claimCustomerId.Value);
        }
        await _HistoricService.Add(dto);
    }
    /// <summary>
    /// Delete a history
    /// </summary>
    /// <param name="id">historic Identifier</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("Delete")]
    public async Task Delete(int id)
    {
        await _HistoricService.Delete(id);
    }
    /// <summary>
    /// Get leader board
    /// </summary>
    /// <returns>List of customer with total impact info</returns>
    [HttpGet]
    [Route("GetLeaderboard")]
    public async Task<Response<GetLeaderboardDto>> GetLeaderboard()
    {
        Guid customerId = Guid.Empty;
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            var claimCustomerId = identity.FindFirst(ClaimTypes.PrimarySid);
            if (claimCustomerId != null)
                customerId = Guid.Parse(claimCustomerId.Value);
        }
        var result = await _HistoricService.GetLeaderboard(customerId);
        return new Response<GetLeaderboardDto>
        {
            Data = result,
            Succeeded = true,
            Message = "Successful operation"
        };
    }
    /// <summary>
    /// Get weekly impact
    /// </summary>
    /// <returns>Weekly impact</returns>
    [HttpGet]
    [Route("GetMyWeeklyTrend")]
    public async Task<Response<IList<GetWeeklyTrendDto>>> GetMyWeeklyTrend()
    {
        Guid customerId = Guid.Empty;
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            var claimCustomerId = identity.FindFirst(ClaimTypes.PrimarySid);
            if (claimCustomerId != null)
                customerId = Guid.Parse(claimCustomerId.Value);
        }
        var result = await _HistoricService.GetMyWeeklyTrend(customerId);
        return new Response<IList<GetWeeklyTrendDto>>
        {
            Data = result,
            Succeeded = true,
            Message = "Successful operation"
        };
    }
    /// <summary>
    /// Get yearly impact
    /// </summary>
    /// <returns>yearly impact</returns>
    [HttpGet]
    [Route("GetYearlyTrend")]
    public async Task<Response<IList<GetYearlyTrendDto>>> GetYearlyTrend()
    {
        Guid customerId = Guid.Empty;
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            var claimCustomerId = identity.FindFirst(ClaimTypes.PrimarySid);
            if (claimCustomerId != null)
                customerId = Guid.Parse(claimCustomerId.Value);
        }
        var result = await _HistoricService.GetYearlyTrend(customerId);
        return new Response<IList<GetYearlyTrendDto>>
        {
            Data = result,
            Succeeded = true,
            Message = "Successful operation"
        };
    }
    #endregion
}