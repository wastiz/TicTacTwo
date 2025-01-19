using System.Text.Json;
using DAL.DTO;
using Microsoft.IdentityModel.Tokens;

namespace DAL;

public class SessionRepositoryJson
{
    
    public List<SessionNameAndId> GetAllSessionNamesWithStates()
    {
        var sessionObjects= new List<SessionNameAndId>();
        var files = System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.SessionExtension);

        foreach (var file in files)
        {
            var sessionJsonStr = System.IO.File.ReadAllText(file);
            var session = System.Text.Json.JsonSerializer.Deserialize<GameSession>(sessionJsonStr);

            if (session != null)
            {
                var sessionObject = new SessionNameAndId
                {
                    SessionId = session.Id,
                    SessionName = session.Name,
                };
                sessionObjects.Add(sessionObject);
            }
        }

        return sessionObjects;
    }
    
    public List<GameSession> GetAllSessions()
    {
        var sessions = new List<GameSession>();
        var files = System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.SessionExtension);

        foreach (var file in files)
        {
            var sessionJsonStr = System.IO.File.ReadAllText(file);
            var session = System.Text.Json.JsonSerializer.Deserialize<GameSession>(sessionJsonStr);

            if (session != null)
            {
                sessions.Add(session);
            }
        }

        return sessions;
    }
    
    public GameSession? GetSessionById (string sessionId)
    {
        var sessionPath = FileHelper.BasePath + sessionId + FileHelper.SessionExtension;

        if (System.IO.File.Exists(sessionPath))
        {
            var sessionJsonStr = System.IO.File.ReadAllText(sessionPath);
            var session = System.Text.Json.JsonSerializer.Deserialize<GameSession>(sessionJsonStr);
            return session;
        }
        else
        {
            return null;
        }
    }
    
    public GameSession CreateSession(GameConfiguration config)
    {
        if (config == null)
        {
            throw new ArgumentNullException(nameof(config), "Session cannot be null");
        }

        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };
        
        var newSession = new GameSession
        {
            GameConfig = config
        };
        
        var sessionJsonStr = System.Text.Json.JsonSerializer.Serialize(newSession, options);
        
        var filePath = FileHelper.BasePath + newSession.Id + FileHelper.SessionExtension;
        
        System.IO.File.WriteAllText(filePath, sessionJsonStr);

        return newSession;
    }


    public void SaveGameState(GameState gameState, string sessionId, string? sessionName)
    {
        var session = GetSessionById(sessionId);
        if (session == null)
        {
            throw new Exception("Session not found");
        }
        
        session.GameState = gameState;
        session.Name = sessionName.IsNullOrEmpty() ? "Autosave " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : sessionName;
        session.LastSaveAt = DateTime.Now;
        
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
            WriteIndented = true
        };
        
        var sessionJsonStr = System.Text.Json.JsonSerializer.Serialize(session, options);
        
        var filePath = FileHelper.BasePath + sessionId + FileHelper.SessionExtension;
        System.IO.File.WriteAllText(filePath, sessionJsonStr);
    }


}