using System.CodeDom;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataLayer.DBContext;

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
        this.Id = id;
        this.Picture = picture;
        this.Username = username;
        this.BirthDate = birthDate;
        this.Email = email;
        this.Password = password;
        this.Role = role;
    }

    public User(byte[] picture, string username, DateTime? birthDate, string email, Role role)
    {
        this.Picture = picture;
        this.Username = username;
        this.BirthDate = birthDate;
        this.Email = email;
        this.Role = role;
    }

    public User(string username, string password)  //Log in
    {
        this.Username = username;
        this.Password = password;
    }

    public User(string username, string email, string password) //Register
    {
        this.Username = username;
        this.Email = email;
        this.Password = password;
    }

    public User(string username, DateTime? birthDate, string email, Role role) //dto
    {
        this.Username = username;
        this.BirthDate = birthDate;
        this.Email = email;
        this.Role = role;
    }

    public User()
    {
        
    }
}
