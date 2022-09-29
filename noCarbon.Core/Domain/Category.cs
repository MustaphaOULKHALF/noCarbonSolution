namespace noCarbon.Core.Domain;

public partial class Category : BaseEntity, ISoftDeletedEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] Icon { get; set; }
    public bool Deleted { get; set; } = false;
    public DateTime? DeletedDate { get; set; }
    public virtual ICollection<Actions> Actions { get; set; }
    public virtual ICollection<Historic> Historics { get; set; }
}
