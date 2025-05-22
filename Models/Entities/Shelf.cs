namespace Models.Entities
{
    public class Shelf
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public User User { get; protected set; }

        public Shelf(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public Shelf(int id, string name, User user)
        {
            this.Id = id;
            this.Name = name;
            this.User = user;
        }

        public Shelf(string name, User user)
        {
            this.Name=name;
            this.User = user;
        }

        public Shelf(string name)
        {
            this.Name = name;
        }

        public Shelf()
        {
            
        }
    }
}
