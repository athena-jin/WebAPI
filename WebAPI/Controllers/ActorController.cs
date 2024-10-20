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
    public class ActorController : ControllerBase
    {
        private readonly CustomDbContext _context;
        #endregion

        public ActorController(CustomDbContext context)
        {
            _context = context;

            if (_context.Actors.Count() == 0)
            {
                // Create a new Actor if collection is empty,
                // which means you can't delete all Actors.
                _context.Actors.Add(new Actor { Name = "Actor1" });
                _context.SaveChanges();
            }
        }

        #region snippet_GetAll
        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetTodoItems()
        {
            return await _context.Actors.ToListAsync();
        }

        #region snippet_GetByID
        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetTodoItem(long id)
        {
            var todoItem = await _context.Actors.FindAsync(id);

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
        public async Task<ActionResult<Actor>> PostTodoItem(Actor item)
        {
            _context.Actors.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }
        #endregion

        #region snippet_Update
        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, Actor item)
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
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.Actors.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Actors.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
#endif
