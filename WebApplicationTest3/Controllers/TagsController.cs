using Microsoft.AspNetCore.Mvc;
using WebApplicationTest3.Models.RequestModels;
using WebApplicationTest3.Services;

namespace WebApplicationTest3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly TagService _tagService;

        public TagsController(TagService tagService) => _tagService = tagService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            return tag == null ? NotFound() : Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newTagId = await _tagService.CreateTagAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newTagId }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTagRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _tagService.UpdateTagAsync(id, request);
            return updated ? NoContent() : NotFound();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _tagService.DeleteTagAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
