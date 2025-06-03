using System.CodeDom;
using System.ComponentModel.DataAnnotations.Schema;
using EFDataLayer.DBContext;

namespace EFDataLayer.Entities;

public partial class User
{
    public int Id { get; set; }

    public byte[]? Picture { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? BirthDate { get; set; }
    public int RoleId { get; set; }

    [NotMapped]
    public Role Role
    {
        get => (Role)RoleId;
        set => RoleId = (int)value;
    }

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();

    public virtual ICollection<CurrentBook> UserBookProgresses { get; set; } = new List<CurrentBook>();

    public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();


    public User(int id, byte[] picture, string username, DateTime? birthDate, string email, string password, Role role)
    {
        Id = id;
        Picture = picture;
        Username = username;
        BirthDate = birthDate;
        Email = email;
        Password = password;
        Role = role;
    }

    public User(byte[] picture, string username, DateTime? birthDate, string email, Role role)
    {
        Picture = picture;
        Username = username;
        BirthDate = birthDate;
        Email = email;
        Role = role;
    }

    public User(string username, string password)  //Log in
    {
        Username = username;
        Password = password;
    }

    public User(string username, string email, string password) //Register
    {
        Username = username;
        Email = email;
        Password = password;
    }

    public User(string username, DateTime? birthDate, string email, Role role) //dto
    {
        Username = username;
        BirthDate = birthDate;
        Email = email;
        Role = role;
    }

    public User()
    {

    }
}
