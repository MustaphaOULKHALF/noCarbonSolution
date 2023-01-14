using AutoMapper;
using noCarbon.API.Models;
using noCarbon.Core.Dtos;

namespace noCarbon.API.Infrastracture.Mappers;

/// <summary>
/// Reperesent mapping from inputs to dto
/// </summary>
public class InputToDto : Profile
{
    /// <summary>
    /// default Ctor 
    /// </summary>
    public InputToDto()
    {
        CreateMap<AddCustomerInput, AddCustomerDto>();
        CreateMap<RefreshTokenInput, LoginResult>();

        CreateMap<ActionsInput, ActionsDto>();
        CreateMap<CategoryInput, CategoryDto>();
        CreateMap<AddHistoricDto, HistoricDto>();
    }
}
