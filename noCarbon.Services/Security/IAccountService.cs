using noCarbon.Core.Domain;
using noCarbon.Core.Dtos;
using System.Security.Claims;

namespace noCarbon.Services.Security;

public partial  interface IAccountService
{
    Task<LoginResult> Login(string username, string password);
    Task<ProfileDto> GetProfileById(Guid customerId);
    Task<Customer> GetById(Guid customerId);
    Task Register(AddCustomerDto add);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    Task<CustomerRefreshToken> GetCustomerRefresh(string username, string token);
    Task AddRefreshToken(string username, string refreshToken);
    Task Delete(int CustomerRefreshTokenId);
    Task<LoginResult> Refresh(string username, string customerId);
}
