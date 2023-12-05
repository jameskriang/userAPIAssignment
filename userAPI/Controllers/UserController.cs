using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using userAPI.Models;
using userAPI.Services;

namespace userAPI.Controller;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll() =>
        await _userService.GetAll();

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        await _userService.Add(user);
        return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        user.Id = id;
        var existingUser = await _userService.Get(id);

        if (existingUser is null)
            return NotFound();

        await _userService.Update(user);
        return Ok();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<User>>> Search([FromQuery, BindRequired] string name) => await _userService.Search(name);

    [HttpGet("secret")]
    [Authorize]
    public async Task<IActionResult> Secret() => Ok();

}
