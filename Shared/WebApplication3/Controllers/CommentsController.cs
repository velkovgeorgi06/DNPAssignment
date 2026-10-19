namespace WebApplication3.Controllers;

using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public CommentsController(ICommentRepository commentRepo, IPostRepository postRepo, IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> Create([FromBody] CreateCommentDto dto)
    {
        await postRepo.GetSingleAsync(dto.PostId);
        await userRepo.GetSingleAsync(dto.UserId);

        Comment comment = new Comment { Body = dto.Body, UserId = dto.UserId, PostId = dto.PostId };
        Comment created = await commentRepo.AddAsync(comment);
        return CreatedAtAction(nameof(GetSingle), new { id = created.Id }, created);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Comment>> GetMany([FromQuery] int? postId, [FromQuery] int? userId)
    {
        IQueryable<Comment> query = commentRepo.GetMany();

        if (postId.HasValue) query = query.Where(c => c.PostId == postId.Value);
        if (userId.HasValue) query = query.Where(c => c.UserId == userId.Value);

        return Ok(query.ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Comment>> GetSingle([FromRoute] int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            return Ok(comment);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Comment comment)
    {
        if (id != comment.Id) return BadRequest("ID mismatch.");

        try
        {
            await commentRepo.UpdateAsync(comment);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }
}