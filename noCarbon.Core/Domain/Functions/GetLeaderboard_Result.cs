namespace noCarbon.Core.Domain.Functions;

public partial class GetLeaderboard_Result
{
    public int? Classement { get; set; }
    public Guid CustomerId { get; set; }
    public string UserName { get; set; }
    public int Balance { get; set; }
    public decimal TotalImpact { get; set; }
}
