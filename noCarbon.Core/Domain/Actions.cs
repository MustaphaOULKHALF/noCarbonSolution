namespace noCarbon.Core.Domain;

public partial class Actions : BaseEntity, ISoftDeletedEntity
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }
    public decimal ReducedCarb { get;set; }
    public bool Deleted { get; set; } = false;
    public DateTime? DeletedDate { get; set; }
    public virtual Category Category { get; set; }
    public ICollection<Historic> Historics { get; set; }
}
