namespace noCarbon.Core.Domain;

public partial class Customer: BaseEntity, ISoftDeletedEntity
{
    public new Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public byte[] Avatar { get; set; }
    public string Location { get; set; }
    public bool AllowNotification { get; set; }
    public bool Deleted { get; set; } = false;
    public DateTime? DeletedDate { get; set; }
    public ICollection<Sponsorship> Sponsorships { get; set; }
    public ICollection<Historic> Historics { get; set; }
}
