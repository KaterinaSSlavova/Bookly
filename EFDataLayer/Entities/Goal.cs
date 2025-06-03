using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EFDataLayer.Entities;

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

    [NotMapped]
    public virtual Status Status
    {
        get => (Status)StatusId;
        set => StatusId = (int)value;
    }

    public virtual User? User { get; set; }

    public Goal(int id, DateTime start, DateTime end, int readingGoal, int currentProgress, Status status, User user)
    {
        this.Id = id;
        this.Start = start;
        this.End = end;
        this.ReadingGoal = readingGoal;
        this.CurrentProgress = currentProgress;
        this.Status = status;
        this.User = user;
    }

    public Goal(int id, DateTime start, DateTime end, int readingGoal, int currentProgress, Status status, int userId)
    {
        this.Id = id;
        this.Start = start;
        this.End = end;
        this.ReadingGoal = readingGoal;
        this.CurrentProgress = currentProgress;
        this.Status = status;
        this.UserId = userId;   
    }

    public Goal(DateTime start, DateTime end, int readingGoal, int userId)
    {
        this.Start = start;
        this.End = end;
        this.ReadingGoal = readingGoal;
        this.CurrentProgress = 0;
        this.StatusId = 1;
        this.UserId = userId;
    }

    public Goal ()
    {

    }
}
