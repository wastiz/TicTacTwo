using DAL.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.GameSessionDtos;
using System.Security.Claims;
using DAL;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/game-sessions")]
[Authorize]
public class GameSessionController : ControllerBase
{
    private readonly ISessionRepository _sessionRepository;

    public GameSessionController(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost("create")]
    public ActionResult<CreateSessionResponse> CreateSession([FromBody] CreateSessionRequest request)
    {
        var userId = GetUserId();
        
        var session = _sessionRepository.CreateGameSession(request.ConfigId, request.GameMode, userId, request.Password);
        return Ok(new { SessionId = session.Id });
    }

    [HttpGet("{sessionId}")]
    public ActionResult<GameSession> GetSession(string sessionId)
    {
        var session = _sessionRepository.GetSessionById(sessionId);
        if (session == null) return NotFound();

        return Ok(session);
    }

    [HttpGet("user")]
    public ActionResult<List<GameSession>> GetUserSessions()
    {
        var userId = GetUserId();
        var sessions = _sessionRepository.GetUserSessionDto(userId);
        return Ok(sessions);
    }

    [HttpGet("{sessionId}/state")]
    public ActionResult<object> GetGameState(string sessionId)
    {
        var (config, state) = _sessionRepository.GetGameState(sessionId);
        return Ok(new { config, state });
    }

    [HttpPost("{sessionId}/join")]
    public IActionResult JoinSession(string sessionId)
    {
        var userId = GetUserId();
        var session = _sessionRepository.GetSessionById(sessionId);

        if (session == null) return NotFound();

        _sessionRepository.SaveSecondPlayer(session, userId);
        return Ok();
    }

    [HttpPost("{sessionId}/save-game")]
    public IActionResult SaveGameState(string sessionId, [FromBody] GameState state)
    {
        _sessionRepository.SaveGameState(state, sessionId);
        return Ok();
    }

    [HttpPost("{sessionId}/name")]
    public IActionResult SaveSessionName(string sessionId, [FromBody] string name)
    {
        _sessionRepository.SaveSessionName(sessionId, name);
        return Ok();
    }

    [HttpDelete("{sessionId}")]
    public IActionResult DeleteSession(string sessionId)
    {
        _sessionRepository.DeleteSession(sessionId);
        return NoContent();
    }
}
