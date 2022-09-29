namespace noCarbon.Core.Domain;

public partial class Historic : BaseEntity
{
    public Guid CustomerId { get; set; }
    public int CategoryId { get; set; }
    public int ActionId { get; set; }
    public int Points { get; set; }
    public decimal ReducedCarb { get; set; }
    public DateTime OperationDate { get; set; } = DateTime.UtcNow.Date;
    public TimeSpan OperationTime { get; set; } = DateTime.UtcNow.TimeOfDay;
    public virtual Customer Customer { get; set; }
    public virtual Category Category { get; set; }
    public virtual Actions Action { get; set; }
}
