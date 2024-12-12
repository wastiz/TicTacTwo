using DAL.DTO;

namespace DAL;

public interface IStateRepository
{
    List<GameState> GetAllGameStates();
    GameState GetGameStateById(string name);
    void SaveGameState(GameState gameState);
    void DeleteGameState(string stateName);
}