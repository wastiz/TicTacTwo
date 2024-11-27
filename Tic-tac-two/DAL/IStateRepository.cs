namespace DAL;

public interface IStateRepository
{
    List<string> GetAllStateNames();
    List<GameState> GetAllGameStates();
    GameState GetGameStateByName(string name);
    void SaveGameState(GameState gameState);
    void DeleteGameState(string stateName);
}