namespace noCarbon.Core.Dtos;

public partial class ActionsDto
{
    public int? Id { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }
    public decimal ReducedCarb { get; set; }
}
