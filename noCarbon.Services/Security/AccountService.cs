using Microsoft.IdentityModel.Tokens;
using noCarbon.Core.Configurations;
using noCarbon.Core.Domain;
using noCarbon.Core.Dtos;
using noCarbon.Core.Exceptions;
using noCarbon.Core.Helpers;
using noCarbon.Data;
using noCarbon.Data.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace noCarbon.Services.Security;

public partial class AccountService : IAccountService
{
    #region Fields
    private readonly AppSettings _appSettings;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<CustomerRefreshToken> _customerRefreshToeknRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly AppDbContext _appDbContext;
    #endregion
    #region Ctor
    public AccountService(AppSettings appSettings,
        IRepository<Customer> customerRepository,
        IRepository<CustomerRefreshToken> customerRefreshToeknRepository,
        IEncryptionService encryptionService,
        AppDbContext appDbContext)
    {
        this._tokenHandler = new JwtSecurityTokenHandler();
        this._appSettings = appSettings;
        this._customerRepository = customerRepository;
        this._encryptionService = encryptionService;
        this._appDbContext = appDbContext;
        this._customerRefreshToeknRepository = customerRefreshToeknRepository;
    }

    #endregion

    public virtual async Task<Customer> GetById(Guid customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer is null)
            throw new UserNotFoundException("the user not found or deleted");
        return customer;
    }
    public virtual async Task<ProfileDto> GetProfileById(Guid customerId)
    {
        var customer = await _customerRepository.Table.FirstOrDefaultAsync(x => x.Id == customerId);
        if (customer is null)
            throw new UserNotFoundException("the user not found or deleted");
        var balanceDetail = await _appDbContext.GetBalance(customerId);
        var result = new ProfileDto()
        {
            FullName = customer.UserName,
            AllowNotification = customer.AllowNotification,
            Avatar = (customer.Avatar != null && customer.Avatar.Length > 0) ? Convert.ToBase64String(customer.Avatar) : "",
            GetBalance = new GetBalanceDto { Balance = balanceDetail.Balance, TotalImpact = balanceDetail.TotalImpact }
        };
        return result;
    }
    public virtual async Task<LoginResult> Login(string username, string password)
    {
        string _password = string.Empty;
        try
        {
            _password = HashHelper.Sha256_hash(_encryptionService.DecryptText(password));
        }
        catch
        {
            throw new FailedLoginException("The username or password incorrect");
        }
        var customer = await _customerRepository.Table.Where(x => x.UserName == username && x.Password == _password).FirstOrDefaultAsync();
        if (customer is null)
            throw new FailedLoginException("The username or password incorrect");
        var tokenKey = Encoding.UTF8.GetBytes(_appSettings.SecurityConfig.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] {
                 new Claim(ClaimTypes.Name, username),
                 new Claim(ClaimTypes.PrimarySid, customer.Id.ToString())
             }),
            Expires = DateTime.Now.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        var refreshToken = GenerateRefreshToken();
        var result = new LoginResult { AccessToken = _tokenHandler.WriteToken(token), RefreshToken = refreshToken };
        await _customerRefreshToeknRepository.InsertAsync(new CustomerRefreshToken { 
            UserName = username, RefreshToken = refreshToken
        });
        return result;
    }
    public virtual async Task Register(AddCustomerDto add)
    {
        try
        {
            if (_customerRepository.Table.Where(x => x.UserName.ToLower().Trim() == add.UserName.ToLower().Trim()).Any())
                throw new DataAlreadyExistException("the user name is already taken");
            var customer = new Customer()
            {
                UserName = add.UserName,
                Password = HashHelper.Sha256_hash(_encryptionService.DecryptText(add.Password)),
                Email = add.Mail,
                AllowNotification = true,
            };
            await _customerRepository.InsertAsync(customer);
            throw new SuccessfulOperationException("Successful operation");
        }
        catch (Exception)
        {
            throw;
        }

    }

    public virtual async Task<CustomerRefreshToken> GetCustomerRefresh(string username, string token)
    {
        var result = await _customerRefreshToeknRepository.Table.Where(e=> e.UserName.ToLower().Trim() == username.ToLower().Trim() && e.RefreshToken == token).FirstOrDefaultAsync();
        if (result == null)
            throw new EntityNotFoundException(string.Format("cannot find an entity {0} with the identifier {1} ", typeof(CustomerRefreshToken), token));
        return result;
    }

    public async Task<LoginResult> Refresh(string username,string customerId)
    {
        var tokenKey = Encoding.UTF8.GetBytes(_appSettings.SecurityConfig.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
          {
                 new Claim(ClaimTypes.Name, username),
                 new Claim(ClaimTypes.PrimarySid, customerId)
          }),
            Expires = DateTime.Now.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        var refreshToken = GenerateRefreshToken();
        return new LoginResult { AccessToken = _tokenHandler.WriteToken(token), RefreshToken = refreshToken };
    }
    public virtual async Task Delete(int CustomerRefreshTokenId)
    {
       if(CustomerRefreshTokenId == 0)
            throw new ArgumentNullException(nameof(CustomerRefreshTokenId));
       var customerRefreshToken=  _customerRefreshToeknRepository.GetById(CustomerRefreshTokenId);
       _customerRefreshToeknRepository.Delete(customerRefreshToken);

    }

    protected virtual string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var Key = Encoding.UTF8.GetBytes(_appSettings.SecurityConfig.Key);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ClockSkew = TimeSpan.Zero
        };
        var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }

    public async Task AddRefreshToken(string username, string refreshToken)
    {
        await _customerRefreshToeknRepository.InsertAsync(new CustomerRefreshToken
        {
            UserName = username,
            RefreshToken = refreshToken
        });
    }
}
