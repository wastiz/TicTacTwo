using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL;
using DAL.Contracts;
using Domain;
using Shared;
using Shared.GameDtos;

namespace API.Controllers;

[ApiController]
[Route("api/game")]
[Authorize]
public class GameController : ControllerBase
{
    private readonly ISessionRepository _sessionRepository;

    public GameController(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    [HttpPost("{sessionId}/place-chip")]
    public IActionResult ClickCell(string sessionId, [FromBody] PlaceChipRequest request)
    {
        var session = _sessionRepository.GetDomainSessionById(sessionId);
        if (session == null) return NotFound("Session not found");

        var brain = new GameBrain(session.GameConfiguration, session.GameState);
        Response response = brain.PlaceChip(request.X, request.Y);
        
        if (response.Success)
        {
            var state = brain.SaveGame();
            _sessionRepository.SaveGameState(state, sessionId);
            return Ok(state);
        }
        
        return BadRequest(response);
    }

    [HttpPost("{sessionId}/move-board")]
    public IActionResult MoveBoard(string sessionId, [FromBody] MoveBoardRequest request)
    {
        var session = _sessionRepository.GetDomainSessionById(sessionId);
        if (session == null) return NotFound("Session not found");

        var brain = new GameBrain(session.GameConfiguration, session.GameState);
        Response response = brain.MoveMovableBoard(request.Direction);

        if (response.Success)
        {
            var state = brain.SaveGame();
            _sessionRepository.SaveGameState(state, sessionId);
            return Ok(state);
        }
        
        return BadRequest(response);
    }
    
    [HttpPost("{sessionId}/move-chip")]
    public IActionResult MoveChip(string sessionId, [FromBody] MoveChipRequest request)
    {
        var session = _sessionRepository.GetDomainSessionById(sessionId);
        if (session == null) return NotFound("Session not found");

        var brain = new GameBrain(session.GameConfiguration, session.GameState);
        Response response = brain.MoveChip(request.StartX, request.StartY, request.EndX, request.EndY);

        if (response.Success)
        {
            var state = brain.SaveGame();
            _sessionRepository.SaveGameState(state, sessionId);
            return Ok(state);
        }
        
        return BadRequest(response);
    }
}
