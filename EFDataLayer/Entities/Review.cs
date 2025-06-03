using System;
using System.Collections.Generic;
using EFDataLayer.Entities;

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

    public Review(int id, string description, DateTime date, User user, Book book)
    {
         this.Id = id;
        this.Description = description;
        this.Date = date;
        this.User = user;
        this.Book = book;
    }

    public Review(int id, string description, DateTime date, int userId, int bookId)
    {
        this.Id = id;
        this.Description = description;
        this.Date = date;
        this.UserId = userId;
        this.BookId = bookId;
    }

    public Review()
    {
        
    }
}
