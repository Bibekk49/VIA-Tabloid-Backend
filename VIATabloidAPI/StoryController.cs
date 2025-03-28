using EFC;
using EFC.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VIATabloidAPI;

[Route("api/[controller]")]
[ApiController]
public class StoryController : ControllerBase
{
    private readonly VIATabloidContext _context;

    public StoryController(VIATabloidContext context)
    {
        _context = context;
    }

    // GET: api/Story
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Story>>> GetStories()
    {
        // Fetches all stories from the database
        return await _context.Stories.ToListAsync();
    }

    // GET: api/Story/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Story>> GetStory(long id)
    {
        // Fetches a single story by its Id
        var story = await _context.Stories.FindAsync(id);

        if (story == null)
        {
            return NotFound();
        }

        return story;
    }

    // POST: api/Story
    [HttpPost]
    public async Task<ActionResult<Story>> PostStory(Story story)
    {
        // Adds a new story to the database
        _context.Stories.Add(story);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStory), new { id = story.Id }, story);
    }

    // PUT: api/Story/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStory(long id, Story story)
    {
        if (id != story.Id)
        {
            return BadRequest();
        }

        _context.Entry(story).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StoryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Story/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStory(long id)
    {
        // Finds the story to delete
        var story = await _context.Stories.FindAsync(id);
        if (story == null)
        {
            return NotFound();
        }

        // Removes the story from the database
        _context.Stories.Remove(story);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StoryExists(long id)
    {
        // Checks if a story exists in the database by its Id
        return _context.Stories.Any(e => e.Id == id);
    }
}