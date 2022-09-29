namespace noCarbon.Core.Domain;

public partial class CustomerRefreshToken : BaseEntity
{
    public string UserName { get; set; }
    public string RefreshToken { get; set; }
    public bool IsActive { get; set; } = true;
}
