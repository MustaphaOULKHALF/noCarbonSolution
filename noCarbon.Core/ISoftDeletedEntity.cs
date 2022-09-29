namespace noCarbon.Core;

public interface ISoftDeletedEntity
{
    bool Deleted { get; set; }
    DateTime? DeletedDate { get; set; }
}
