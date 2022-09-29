using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using noCarbon.API.Infrastracture;
using noCarbon.API.Models;
using noCarbon.Core.Dtos;
using noCarbon.Services.Action;

namespace noCarbon.API.Controllers;
/// <summary>
/// represent actions controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ActionsController : ControllerBase
{
    #region Fields
    private readonly IActionsService _actionsService;
    private readonly IMapper _mapper;
    #endregion
    #region Ctor
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="actionsService">actions service</param>
    /// <param name="mapper">auto mapper</param>
    public ActionsController(IActionsService actionsService,
        IMapper mapper)
    { 
        this._actionsService = actionsService;
        this._mapper = mapper;
    }
    #endregion
    #region Methods
    /// <summary>
    /// Get all actions, and also you can have actions by specific category
    /// </summary>
    /// <param name="CategoryId">category identifier</param>
    /// <returns>action list</returns>
    [HttpGet]
    [Route("GetAll")]
    public async Task<Response<IList<ActionsDto>>> GetAll(int? CategoryId)
    {
        var result = await _actionsService.GetAll(CategoryId);
        return new Response<IList<ActionsDto>>
        {
            Data = result,
            Succeeded = true,
            Message = "Successful operation"
        };
    }
    /// <summary>
    /// Add new action
    /// </summary>
    /// <param name="input">action info</param>
    /// <returns></returns>
    [HttpPost]
    [Route("Add")]
    public async Task Add(ActionsInput input)
    {
        await _actionsService.Add(_mapper.Map<ActionsDto>(input));
    }
    /// <summary>
    /// Delete an action
    /// </summary>
    /// <param name="id">action identifier</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("Delete")]
    public async Task Delete(int id)
    {
        await _actionsService.Delete(id);
    }
    /// <summary>
    /// Update an action
    /// </summary>
    /// <param name="input">action with new info</param>
    /// <param name="id">Action identifier</param>
    /// <returns></returns>
    [HttpPut]
    [Route("Update")]
    public async Task Update(ActionsInput input, int id)
    {
        await _actionsService.Update(_mapper.Map<ActionsDto>(input), id);
    }
    #endregion
}
