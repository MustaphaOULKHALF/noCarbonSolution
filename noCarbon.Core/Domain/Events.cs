using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace noCarbon.Core.Domain;

public partial class Events :BaseEntity, ISoftDeletedEntity
{
    public string Title { get; set; }   
    public string Body { get; set; }
    public byte[] Image { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool Deleted { get; set; } = false;
    public DateTime? DeletedDate { get; set; }
}
