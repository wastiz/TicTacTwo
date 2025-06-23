using DAL.Contracts;
using DAL.Contracts.Interfaces;
using DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShiftEaseAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;
    
    public AuthController(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    //User Registration
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegister model)
    {
        if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            return BadRequest("All fields are required.");

        var result = await _repository.CreateUser(model);
        if (!result.Success) return BadRequest(result.Message);

        return Ok(new
        {
            accessToken = JwtTokenHelper.GenerateToken(result.Data.Id, result.Data.Username, _configuration),
            message = result.Message
        });
    }

    //User Login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLogin model)
    {
        var result = await _repository.CheckPassword(model);
        if (!result.Success) return Unauthorized(result.Message);
        return Ok(new
        {
            accessToken = JwtTokenHelper.GenerateToken(result.Data.Id, result.Data.Username, _configuration),
            message = result.Message
        });
    }
}
