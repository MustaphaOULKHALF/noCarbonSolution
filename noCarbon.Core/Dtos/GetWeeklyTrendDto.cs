namespace noCarbon.Core.Dtos;

public partial class GetWeeklyTrendDto
{
    public DateTime DayOfTheWeek { get; set; }
    public decimal TotalImpact { get; set; }
}