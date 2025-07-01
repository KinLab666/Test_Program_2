using System.ComponentModel.DataAnnotations;

namespace WebApplicationTest3.Models.RequestModels
{
    public class CreateTaskRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Deadline { get; set; }

        public List<int> TagIds { get; set; } = new();
    }
}
