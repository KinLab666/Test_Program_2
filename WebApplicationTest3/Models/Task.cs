namespace WebApplicationTest3.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Deadline { get; set; }
        public bool Completed { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
