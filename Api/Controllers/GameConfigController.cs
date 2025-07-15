using DAL.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Shared;
using Shared.GameConfigDtos;

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
    public async Task<ActionResult<List<GameConfigDto>>> GetAllConfigsByUserId()
    {
        var userId = GetUserIdFromToken();
        if (userId == null) return Ok("User ID not found in token.");

        var configs = await _repository.GetAllUserConfigDto(userId);
        return Ok(configs);
    }

    [HttpGet("{configId}")]
    public async Task<ActionResult<GameConfigDto>> GetConfigById(string configId)
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
    public async Task<ActionResult<GameConfigDto>> CreateConfig([FromBody] GameConfigDto configDto)
    {
        var userId = GetUserIdFromToken();
        if (userId == null) return Unauthorized("User ID not found in token.");

        var response = await _repository.CreateGameConfiguration(userId, configDto);
    
        if (!response.Success) return BadRequest(response.Message);

        return Ok(response.Data);
    }

    [HttpPut("{configId}")]
    public async Task<ActionResult<Response>> UpdateConfig(string configId, [FromBody] GameConfigDto configDto)
    {
        if (configId != configDto.Id)
            return BadRequest("Config ID in URL does not match ID in body.");

        var response = await _repository.UpdateConfiguration(configId, configDto);
        if (!response.Success) return NotFound(response);

        return Ok(response.Data);
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
