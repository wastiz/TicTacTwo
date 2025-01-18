using System.IdentityModel.Tokens.Jwt;
using DAL;
using DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Configs : PageModel
{
    public string UserId { get; set; }
    [BindProperty] public string Username { get; set; }
    private ConfigRepository _configRepository;

    public Configs(ConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }
    
    [BindProperty] public List<GameConfigDto> gameConfigs { get; set; }
    
    private void SetUserIdFromToken()
    {
        var token = HttpContext.Request.Cookies["authToken"];
        if (!string.IsNullOrEmpty(token))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            Username = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
        }
    }
    
    public void OnGet()
    {
        SetUserIdFromToken();
        gameConfigs = _configRepository.GetAllUserConfigDto(UserId);
    }
    
    public IActionResult OnPostCreate([FromBody] CreateRequest request)
    {
        if (request.BoardSizeWidth > 10 || request.BoardSizeHeight > 10)
        {
            return BadRequest("Board size dimensions cannot exceed 10.");
        }

        if (request.WinCondition > Math.Min(request.BoardSizeWidth, request.BoardSizeHeight))
        {
            return BadRequest("Win condition cannot exceed board dimensions.");
        }

        if (request.OptionsAfterNMoves > request.ChipsCount)
        {
            return BadRequest("Options after N moves cannot exceed chip count.");
        }
        
        SetUserIdFromToken();

        var newConfig = new GameConfiguration
        {
            Name = request.Name,
            BoardSizeWidth = request.BoardSizeWidth,
            BoardSizeHeight = request.BoardSizeHeight,
            WinCondition = request.WinCondition,
            OptionsAfterNMoves = request.OptionsAfterNMoves,
            ChipsCount = new[] { 0, request.ChipsCount, request.ChipsCount },
            CreatedBy = UserId
        };

        _configRepository.CreateGameConfiguration(newConfig);
        return new JsonResult(new { success = true });
    }
    
    public IActionResult OnPostDelete([FromBody] DeleteRequest request)
    {
        _configRepository.DeleteConfiguration(request.ConfigId);
        return new JsonResult(new { success = true });
    }
    
    public IActionResult OnPostUpdate([FromBody] UpdateRequest request)
    {
        try
        {
            if (request.BoardSizeWidth > 10 || request.BoardSizeHeight > 10)
            {
                return BadRequest("Board size dimensions cannot exceed 10.");
            }

            if (request.WinCondition > Math.Min(request.BoardSizeWidth, request.BoardSizeHeight))
            {
                return BadRequest("Win condition cannot exceed board dimensions.");
            }

            if (request.OptionsAfterNMoves > request.ChipsCount)
            {
                return BadRequest("Options after N moves cannot exceed chip count.");
            }
            
            var existingConfig = _configRepository.GetConfigurationById(request.ConfigId);
            if (existingConfig == null)
            {
                return NotFound("Configuration not found.");
            }
            
            existingConfig.Name = request.Name;
            existingConfig.BoardSizeWidth = request.BoardSizeWidth;
            existingConfig.BoardSizeHeight = request.BoardSizeHeight;
            existingConfig.WinCondition = request.WinCondition;
            existingConfig.OptionsAfterNMoves = request.OptionsAfterNMoves;
            existingConfig.ChipsCount = new[] { 0, request.ChipsCount, request.ChipsCount };
            
            _configRepository.UpdateConfiguration(request.ConfigId, existingConfig);

            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnPostUpdate: {ex.Message}");
            return StatusCode(500, "An error occurred while updating the configuration.");
        }
    }
    
    public JsonResult OnGetGetConfig(string configId)
    {
        var config = _configRepository.GetConfigurationById(configId);
        return new JsonResult(new
        {
            name = config.Name,
            boardSizeWidth = config.BoardSizeWidth,
            boardSizeHeight = config.BoardSizeHeight,
            winCondition = config.WinCondition,
            optionsAfterNMoves = config.OptionsAfterNMoves,
            chipsCount = config.ChipsCount[1]
        });
    }
}

public class CreateRequest
{
    public string Name { get; set; }
    public int BoardSizeWidth { get; set; }
    public int BoardSizeHeight { get; set; }
    public int WinCondition { get; set; }
    public int OptionsAfterNMoves { get; set; }
    public int ChipsCount { get; set; }
}

public class UpdateRequest : CreateRequest
{
    public string ConfigId { get; set; }
}

public class DeleteRequest
{
    public string ConfigId { get; set; }
}
