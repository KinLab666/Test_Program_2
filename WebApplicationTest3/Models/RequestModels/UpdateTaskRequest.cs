using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest3.Models.RequestModels
{
    public class UpdateTaskRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Deadline { get; set; }

        public bool? Completed { get; set; }

        public List<int> TagIds { get; set; } = new();
    }
}
