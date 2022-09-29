namespace noCarbon.Core.Domain;

public partial class Sponsorship : BaseEntity, ISoftDeletedEntity
{
    public Guid CustomerId { get; set; }
    public string Token { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Message { get; set; }
    public bool Deleted { get; set; } = false;
    public DateTime? DeletedDate { get; set; }
    public virtual Customer Customer { get; set; }
}
