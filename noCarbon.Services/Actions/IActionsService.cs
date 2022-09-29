using noCarbon.Core.Dtos;

namespace noCarbon.Services.Action;

/// <summary>
/// Action service
/// </summary>
public interface IActionsService
{
    /// <summary>
    /// Get all action list by Category
    /// </summary>
    /// <param name="CategoryId">action category if CategoryId null, you'll get all categories</param>
    /// <returns>action list</returns>
    Task<IList<ActionsDto>> GetAll(int? CategoryId = null);
    /// <summary>
    /// Get a specific action by id
    /// </summary>
    /// <param name="id">action identifier</param>
    /// <returns>action</returns>
    Task<ActionsDto> GetById(int id);
    /// <summary>
    /// add new action
    /// </summary>
    /// <param name="dto">action info</param>
    /// <returns></returns>
    Task Add(ActionsDto dto);
    /// <summary>
    /// Delete an action by identifier
    /// </summary>
    /// <param name="id">action identifier</param>
    /// <returns></returns>
    Task Delete(int id);
    /// <summary>
    /// Update an action by identifier
    /// </summary>
    /// <param name="id">action identifier</param>
    /// <param name="dto">action details</param>
    /// <returns></returns>
    Task Update(ActionsDto dto, int id);
}
