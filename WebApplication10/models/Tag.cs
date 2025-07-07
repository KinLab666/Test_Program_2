using System.ComponentModel.DataAnnotations;

namespace WebApplication10.models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Plan> Plans { get; set; } = new List<Plan>();
    }
}
