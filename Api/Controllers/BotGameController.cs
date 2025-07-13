using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL;
using DAL.Contracts;
using Domain;
using Shared;
using Shared.GameDtos;

namespace API.Controllers;

[ApiController]
[Route("api/bot-game")]
[Authorize]
public class BotGameController : ControllerBase
{
    private readonly ISessionRepository _sessionRepository;
    private readonly Bot _bot;

    public BotGameController(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
        _bot = new Bot(botPlayerNumber: 2, maxDepth: 3);
    }

    [HttpPost("{sessionId}/place-chip")]
    public IActionResult PlaceChip(string sessionId, [FromBody] PlaceChipRequest request)
    {
        var session = _sessionRepository.GetDomainSessionById(sessionId);
        if (session == null) return NotFound("Session not found");
        
        var brain = new GameBrain(session.GameConfiguration, session.GameState);
        Response response;
        
        response = brain.PlaceChip(request.X, request.Y);
        if (!response.Success) return BadRequest(response);
   
        var state = brain.SaveGame();
        _sessionRepository.SaveGameState(state, sessionId);

        if (brain.Win == 0)
        {
            MakeBotMove(brain, sessionId);
        }
        
        return Ok(_sessionRepository.GetDomainSessionById(sessionId)?.GameState);
    }

    [HttpPost("{sessionId}/move-board")]
    public IActionResult MoveBoard(string sessionId, [FromBody] MoveBoardRequest request)
    {
        var session = _sessionRepository.GetDomainSessionById(sessionId);
        if (session == null) return NotFound("Session not found");
        
        var brain = new GameBrain(session.GameConfiguration, session.GameState);
        Response response = brain.MoveMovableBoard(request.Direction);
        
        if (!response.Success) return BadRequest(response);

        var state = brain.SaveGame();
        _sessionRepository.SaveGameState(state, sessionId);

        if (brain.Win == 0)
        {
            MakeBotMove(brain, sessionId);
        }
        
        return Ok(_sessionRepository.GetDomainSessionById(sessionId)?.GameState);
    }
    
    [HttpPost("{sessionId}/move-chip")]
    public IActionResult MoveChip(string sessionId, [FromBody] MoveChipRequest request)
    {
        var session = _sessionRepository.GetDomainSessionById(sessionId);
        if (session == null) return NotFound("Session not found");
        
        var brain = new GameBrain(session.GameConfiguration, session.GameState);
        Response response = brain.MoveChip(request.StartX, request.StartY, request.EndX, request.EndY);
        
        if (!response.Success) return BadRequest(response);
    
        var state = brain.SaveGame();
        _sessionRepository.SaveGameState(state, sessionId);
        
        if (brain.Win == 0)
        {
            MakeBotMove(brain, sessionId);
        }
        
        return Ok(_sessionRepository.GetDomainSessionById(sessionId)?.GameState);
    }
    
    private void MakeBotMove(GameBrain brain, string sessionId)
    {
        Move bestMove = _bot.GetBestMove(brain);
        
        if (bestMove != null)
        {
            Response botResponse;
            switch (bestMove.Type)
            {
                case Move.MoveType.Place:
                    botResponse = brain.PlaceChip(bestMove.X1, bestMove.Y1);
                    break;
                case Move.MoveType.MoveChip:
                    botResponse = brain.MoveChip(bestMove.X1, bestMove.Y1, bestMove.X2, bestMove.Y2);
                    break;
                case Move.MoveType.MoveBoard:
                    botResponse = brain.MoveMovableBoard(bestMove.Direction);
                    break;
                default:
                    botResponse = new Response { Success = false, Message = "Invalid move type" };
                    break;
            }

            if (botResponse.Success)
            {
                var botState = brain.SaveGame();
                _sessionRepository.SaveGameState(botState, sessionId);
            }
        }
    }
}