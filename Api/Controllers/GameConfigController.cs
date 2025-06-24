using DAL.Contracts;
using DAL.Contracts.DTO;
using DAL.DTO.GameConfigDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/game-configs")]
public class GameConfigController : ControllerBase
{
    private readonly IConfigRepository _repository;

    public GameConfigController(IConfigRepository repository)
    {
        _repository = repository;
    }

    private string? GetUserIdFromToken()
    {
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
        }
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        
    }

    [HttpGet("user")]
    public async Task<ActionResult<List<GameConfig>>> GetAllConfigsByUserId()
    {
        var userId = GetUserIdFromToken();
        if (userId == null) return Ok("User ID not found in token.");

        var configs = await _repository.GetAllUserConfigDto(userId);
        return Ok(configs);
    }

    [HttpGet("{configId}")]
    public async Task<ActionResult<GameConfig>> GetConfigById(string configId)
    {
        try
        {
            var config = await _repository.GetConfigurationById(configId);
            return Ok(config);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Response>> CreateConfig([FromBody] GameConfig config)
    {
        var userId = GetUserIdFromToken();
        if (userId == null) return Unauthorized("User ID not found in token.");

        var response = await _repository.CreateGameConfiguration(userId, config);
        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPut("{configId}")]
    public async Task<ActionResult<Response>> UpdateConfig(string configId, [FromBody] GameConfig config)
    {
        if (configId != config.Id)
            return BadRequest("Config ID in URL does not match ID in body.");

        var response = await _repository.UpdateConfiguration(configId, config);
        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    [HttpDelete("{configId}")]
    public async Task<ActionResult<Response>> DeleteConfig(string configId)
    {
        var response = await _repository.DeleteConfiguration(configId);
        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }
}
