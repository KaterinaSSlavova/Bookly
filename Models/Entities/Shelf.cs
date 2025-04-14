namespace Models.Entities
{
    public class Shelf
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

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
