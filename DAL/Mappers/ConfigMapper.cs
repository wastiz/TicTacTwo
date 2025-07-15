using Domain;
using Shared.GameConfigDtos;

namespace DAL.Mappers;

public static class ConfigMapper
{
    public static GameConfigDto ToDto(GameConfiguration config)
    {
        return new GameConfigDto
        {
            Id = config.Id,
            Name = config.Name,
            BoardSizeWidth = config.BoardSizeWidth,
            BoardSizeHeight = config.BoardSizeHeight,
            MovableBoardWidth = config.MovableBoardWidth,
            MovableBoardHeight = config.MovableBoardHeight,
            Player1Chips = config.Player1Chips,
            Player2Chips = config.Player2Chips,
            WinCondition = config.WinCondition,
            AbilitiesAfterNMoves = config.AbilitiesAfterNMoves
        };
    }

    public static GameConfiguration ToDomain(GameConfigDto config)
    {
        return new GameConfiguration()
        {
            Id = config.Id,
            Name = config.Name,
            BoardSizeWidth = config.BoardSizeWidth,
            BoardSizeHeight = config.BoardSizeHeight,
            MovableBoardWidth = config.MovableBoardWidth,
            MovableBoardHeight = config.MovableBoardHeight,
            Player1Chips = config.Player1Chips,
            Player2Chips = config.Player2Chips,
            WinCondition = config.WinCondition,
            AbilitiesAfterNMoves = config.AbilitiesAfterNMoves
        };
    }
}