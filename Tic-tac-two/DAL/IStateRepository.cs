using DAL.DTO;

namespace DAL;

public interface IStateRepository
{
    List<GameStateDto> GetAllStateDto();
    List<GameState> GetAllGameStates();
    GameState GetGameStateById(string name);
    void SaveGameState(GameState gameState);
    void DeleteGameState(string stateName);
}