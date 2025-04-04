namespace Models.Entities
{
    public class Shelf
    {
        public int Id { get; set; }
        public string Name { get; set; }
       // public List<Book> Books { get; set; }

        public Shelf(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public Shelf(string name)
        {
            this.Name=name;
        }
    }
}
