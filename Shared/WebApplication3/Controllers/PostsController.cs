namespace WebApplication3.Controllers;

using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<ActionResult<Post>> Create([FromBody] CreatePostDto dto)
    {
        await userRepo.GetSingleAsync(dto.UserId); // Validate user

        Post post = new Post { Title = dto.Title, Body = dto.Body, UserId = dto.UserId };
        Post created = await postRepo.AddAsync(post);
        return CreatedAtAction(nameof(GetSingle), new { id = created.Id }, created);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Post>> GetMany([FromQuery] int? userId)
    {
        IQueryable<Post> query = postRepo.GetMany();
        if (userId.HasValue)
        {
            query = query.Where(p => p.UserId == userId.Value);
        }
        return Ok(query.ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetSingle([FromRoute] int id, [FromQuery] bool includeComments = false)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            User author = await userRepo.GetSingleAsync(post.UserId);

            if (includeComments)
            {
                var comments = commentRepo.GetMany().Where(c => c.PostId == id).ToList();
                return Ok(new { post, authorName = author.Name, comments });
            }

            return Ok(new { post, authorName = author.Name });
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Post post)
    {
        if (id != post.Id) return BadRequest("ID mismatch.");

        try
        {
            await postRepo.UpdateAsync(post);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");
        }
    }
}