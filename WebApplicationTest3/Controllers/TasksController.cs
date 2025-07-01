using Microsoft.AspNetCore.Mvc;
using WebApplicationTest3.Models.RequestModels;
using WebApplicationTest3.Services;

namespace WebApplicationTest3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService) => _taskService = taskService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _taskService.GetAllTasksAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _taskService.CreateTaskAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _taskService.UpdateTaskAsync(id, request);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
