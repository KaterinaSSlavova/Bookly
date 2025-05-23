using System;
using System.Collections.Generic;

namespace EFDataLayer.DBContext;

public partial class Review
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public DateTime Date { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public bool IsArchived { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
