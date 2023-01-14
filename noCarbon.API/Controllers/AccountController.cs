using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using noCarbon.API.Infrastracture;
using noCarbon.API.Models;
using noCarbon.Core.Dtos;
using noCarbon.Core.Exceptions;
using noCarbon.Services.Security;
using System.Security.Claims;

namespace noCarbon.API.Controllers;

/// <summary>
/// represent account controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    #region Fields
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;
    #endregion
    #region Ctor
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="accountService">account service</param>
    /// <param name="mapper">auto mapper</param>
    public AccountController(IAccountService accountService,
        IMapper mapper)
    {
        this._accountService = accountService;
        this._mapper = mapper;
    }
    #endregion
    #region Methods
    /// <summary>
    /// Login action
    /// </summary>
    /// <param name="input">User information(UserName,Password)</param>
    /// <returns>Login result</returns>
    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<Response<LoginResult>> Login([FromBody] LoginInput input)
    {
        return new Response<LoginResult>
        {
            Data = await _accountService.Login(input.Username, input.Password),
            Succeeded = true,
            Message = "Successful operation"
        };
    }
    /// <summary>
    /// Get profile details
    /// </summary>
    /// <returns>profil details</returns>
    [Authorize]
    [HttpGet]
    [Route("GetProfile")]
    public async Task<Response<ProfileDto>> GetProfile()
    {
        Guid customerId = Guid.Empty;
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity is not null)
        {
            var claimCustomerId = identity.FindFirst(ClaimTypes.PrimarySid);
            if (claimCustomerId != null)
                customerId = Guid.Parse(claimCustomerId.Value);
        }
        var profil = await _accountService.GetProfileById(customerId);
        return new Response<ProfileDto>
        {
            Data = profil,
            Succeeded = true,
            Message = "Successful operation"
        };
    }
    /// <summary>
    /// Register new customer
    /// </summary>
    /// <param name="add">customer info</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("Register")]
    public async Task Register(AddCustomerInput add)
    {
       await _accountService.Register(_mapper.Map<AddCustomerDto>(add));
    }
    /// <summary>
    /// Refresh JWtoken, if the token expired
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="FailedRefreshTokenException"></exception>
    [AllowAnonymous]
    [HttpPost]
    [Route("Refresh")]
    public async Task<LoginResult> Refresh(RefreshTokenInput token)
    {
        var principal = _accountService.GetPrincipalFromExpiredToken(token.AccessToken);
        var username = principal.Identity?.Name;
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        string customerId = string.Empty;
        if (identity is not null)
        {
            var claimCustomerId = identity.FindFirst(ClaimTypes.PrimarySid);
            if (claimCustomerId != null)
                customerId = claimCustomerId.Value;
        }
        //retrieve the saved refresh token from database
        var savedRefreshToken = await _accountService.GetCustomerRefresh(username, token.RefreshToken);
        var newJwtToken = await _accountService.Refresh(username, customerId);
        if (newJwtToken == null)
            throw new FailedRefreshTokenException("");
        await _accountService.Delete(savedRefreshToken.Id);
        await _accountService.AddRefreshToken(username, newJwtToken.RefreshToken);
        return newJwtToken;
    }

    #endregion

}
