namespace WebApplication3.Controllers;

using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create([FromBody] CreateUserDto dto)
    {
        User user = new User { Name = dto.Name, Password = dto.Password };
        User created = await userRepo.AddAsync(user);
        return CreatedAtAction(nameof(GetSingle), new { id = created.Id }, created);
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetMany([FromQuery] string? nameContains)
    {
        IQueryable<User> query = userRepo.GetMany();
        if (!string.IsNullOrWhiteSpace(nameContains))
        {
            query = query.Where(u => u.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }
        return Ok(query.ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetSingle([FromRoute] int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            return Ok(user);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"User with ID {id} not found.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] User user)
    {
        if (id != user.Id) return BadRequest("ID mismatch.");

        try
        {
            await userRepo.UpdateAsync(user);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"User with ID {id} not found.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            await userRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"User with ID {id} not found.");
        }
    }
}