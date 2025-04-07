namespace Models.Entities
{
    public class Shelf
    {
        public int Id { get; }
        public string Name { get; }

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
