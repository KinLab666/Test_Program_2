using System.ComponentModel.DataAnnotations;

namespace WebApplication10.models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public bool Completed { get; set; } = false;
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
