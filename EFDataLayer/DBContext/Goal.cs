using System;
using System.Collections.Generic;

namespace EFDataLayer.DBContext;

public partial class Goal
{
    public int Id { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public int ReadingGoal { get; set; }

    public int CurrentProgress { get; set; }

    public int StatusId { get; set; }

    public int? UserId { get; set; }

    public bool IsArchived { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual User? User { get; set; }
}
