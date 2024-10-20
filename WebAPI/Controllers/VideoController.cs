#define Primary
#if Primary
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

#region TodoController
namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly CustomDbContext _context;
        #endregion

        public VideoController(CustomDbContext context)
        {
            _context = context;

            if (_context.Videos.Count() == 0)
            {
                // Create a new Video if collection is empty,
                // which means you can't delete all Videos.
                _context.Videos.Add(new Video { Name = "Videos" });
                _context.SaveChanges();
            }
        }

        #region snippet_GetAll
        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetTodoItems()
        {
            return await _context.Videos.ToListAsync();
        }

        #region snippet_GetByID
        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetTodoItem(Guid id)
        {
            var todoItem = await _context.Videos.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
        #endregion
        #endregion

        #region snippet_Create
        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<Video>> PostTodoItem(Video item)
        {
            _context.Videos.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }
        #endregion

        #region snippet_Update
        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, Video item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region snippet_Delete
        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            var todoItem = await _context.Videos.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Videos.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region delete all
        [HttpDelete()]
        public async Task<IActionResult> DeleteTodoItem()
        {
            var todoItems = await _context.Videos.ToListAsync();

            if (todoItems == null)
            {
                return NotFound();
            }

            _context.Videos.RemoveRange(todoItems);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
#endif
