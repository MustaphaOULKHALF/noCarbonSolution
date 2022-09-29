namespace noCarbon.Core.Dtos;

public partial class AddHistoricDto
{
    public Guid CustomerId { get; set; }
    public int CategoryId { get; set; }
    public int ActionId { get; set; }
}
