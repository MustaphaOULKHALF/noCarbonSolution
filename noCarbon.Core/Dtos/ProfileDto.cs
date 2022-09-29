namespace noCarbon.Core.Dtos;

public class ProfileDto
{
    public string FullName { get; set; }
    public string Avatar { get;set; }
    public GetBalanceDto GetBalance { get; set; }  
    public bool AllowNotification { get; set; }
}
