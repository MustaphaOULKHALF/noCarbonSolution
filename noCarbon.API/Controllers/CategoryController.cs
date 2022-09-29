using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using noCarbon.API.Infrastracture;
using noCarbon.API.Models;
using noCarbon.Core.Dtos;
using noCarbon.Services.Categories;

namespace noCarbon.API.Controllers;

/// <summary>
/// represent account controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    #region Fields
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    #endregion
    #region Ctor
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="categoryService">category service</param>
    /// <param name="mapper">auto mapper</param>
    public CategoryController(ICategoryService categoryService 
        ,IMapper mapper)
    {
        this._categoryService = categoryService;
        this._mapper = mapper;
    }
    #endregion
    #region Methods
    /// <summary>
    /// Get all category
    /// </summary>
    /// <returns>category list</returns>
    [HttpGet]
    [Route("GetAll")]
    public async Task<Response<IList<CategoryDto>>> GetAll()
    {
        var result = await _categoryService.GetAll();
        return new Response<IList<CategoryDto>>
        {
            Data = result,
            Succeeded = true,
            Message = "Successful operation"
        };
    }

    /// <summary>
    /// add new category
    /// </summary>
    /// <param name="input">category info</param>
    /// <returns></returns>
    [HttpPost]
    [Route("Add")]
    public async Task Add(CategoryInput input)
    {
        await _categoryService.Add(_mapper.Map<CategoryDto>(input));
    }
    /// <summary>
    /// delete a category
    /// </summary>
    /// <param name="id">category identifier</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("Delete")]
    public async Task Delete(int id)
    {
        await _categoryService.Delete(id);
    }
    /// <summary>
    /// update a category
    /// </summary>
    /// <param name="input">new category info</param>
    /// <param name="id">category identifier</param>
    /// <returns></returns>
    [HttpPut]
    [Route("Update")]
    public async Task Update(CategoryInput input, int id)
    {
        await _categoryService.Update(_mapper.Map<CategoryDto>(input), id);
    }
    #endregion

}
