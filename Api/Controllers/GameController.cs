using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL;
using DAL.Contracts;
using Domain;
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

    [HttpPost("{sessionId}/click")]
    public IActionResult ClickCell(string sessionId, [FromBody] CellClickRequest request)
    {
        var session = _sessionRepository.GetDomainSessionById(sessionId);
        if (session == null) return NotFound("Session not found");

        var brain = new GameBrain(session.GameConfiguration, session.GameState);
        bool madeMove = brain.placeChip(request.X, request.Y);
        
        if (madeMove)
        {
            var state = brain.SaveGame();
            _sessionRepository.SaveGameState(state, sessionId);
            return Ok(state);
        }
        
        return Ok("You cannot make this move");
    }

    [HttpPost("{sessionId}/move")]
    public IActionResult MoveBoard(string sessionId, [FromBody] MoveRequest request)
    {
        var session = _sessionRepository.GetDomainSessionById(sessionId);
        if (session == null) return NotFound("Session not found");

        var brain = new GameBrain(session.GameConfiguration, session.GameState);
        var madeMove = brain.moveMovableBoard(request.Direction);

        if (madeMove)
        {
            var state = brain.SaveGame();
            _sessionRepository.SaveGameState(state, sessionId);
            return Ok(state);
        }
        
        return Ok("You cannot move");
    }
}
