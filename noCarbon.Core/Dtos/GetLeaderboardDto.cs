using noCarbon.Core.Domain.Functions;

namespace noCarbon.Core.Dtos;

public partial class GetLeaderboardDto
{
    public GetLeaderboard_Result CurrentCustomer { get; set; }
    public IList<GetLeaderboard_Result> Leaderboards { get; set; }
}
