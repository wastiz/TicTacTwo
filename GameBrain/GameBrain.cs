
using DAL;
using Domain;
using Shared;

public class GameBrain
{
    public int[,] Board;
    public int BoardWidth;
    public int BoardHeight;
    public int[,] MovableBoard;
    public int MovableBoardWidth;
    public int MovableBoardHeight;
    public int GridX;
    public int GridY;
    public int PlayerNumber; //current player's turn (1 or 2)
    public int[] ChipsLeft; //chipsLeft[1] - player 1 chips, chipsLeft[2] - player 2 chips
    public int Player1InitialChips;
    public int Player2InitialChips;
    public int AbilitiesAfterNMoves; // How many chips have player to place to get abilities. 0 - from start abilities are available
    public bool Player1Abilities; // player 1 have abilities?
    public bool Player2Abilities; // player 2 have abilities?
    public int WinCondition;
    public int Win = 0; //0 - nothing, 1 - player 1 won, 2 - player 2 won, 3 - draw
    
    public GameBrain(GameConfiguration config, GameState state)
    {
        Board = new int[config.BoardSizeHeight, config.BoardSizeWidth];
        BoardWidth = config.BoardSizeWidth;
        BoardHeight = config.BoardSizeHeight;
        MovableBoard = new int[config.MovableBoardHeight, config.MovableBoardWidth];
        MovableBoardWidth = config.MovableBoardWidth;
        MovableBoardHeight = config.MovableBoardHeight;
        WinCondition = config.WinCondition;
        AbilitiesAfterNMoves = config.AbilitiesAfterNMoves;
        Player1InitialChips = config.Player1Chips;
        Player2InitialChips = config.Player2Chips;
        
        int rows = state.Board.Length;
        int cols = state.Board[0].Length;
        var mappedBoard = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
          for (int j = 0; j < cols; j++)
          {
            mappedBoard[i, j] = state.Board[i][j];
          }
        }
        
        Board = mappedBoard;
        GridX = state.GridX;
        GridY = state.GridY;
        PlayerNumber = state.PlayerNumber;
        Player1Abilities = state.Player1Abilities;
        Player2Abilities = state.Player2Abilities;
        ChipsLeft = state.ChipsLeft;
        Win = state.Win;
    }
    
    private void CheckForWinner()
    {
        
        if (WinCondition > BoardHeight || WinCondition > BoardWidth)
        {
            return;
        }
        
        for (int i = 0; i < BoardHeight; i++)
        {
            for (int j = 0; j <= BoardWidth - WinCondition; j++)
            {
                bool winConditionMet = true;
                for (int k = 1; k < WinCondition; k++)
                {
                    if (Board[i, j] == 0 || Board[i, j] != Board[i, j + k])
                    {
                        winConditionMet = false;
                        break;
                    }
                }
                if (winConditionMet)
                {
                    Win = PlayerNumber;
                    return;
                }
            }
        }
        
        for (int j = 0; j < BoardWidth; j++)
        {
            for (int i = 0; i <= BoardHeight - WinCondition; i++)
            {
                bool winConditionMet = true;
                for (int k = 1; k < WinCondition; k++)
                {
                    if (Board[i, j] == 0 || Board[i, j] != Board[i + k, j])
                    {
                        winConditionMet = false;
                        break;
                    }
                }
                if (winConditionMet)
                {
                    Win = PlayerNumber;
                    return;
                }
            }
        }
        
        for (int i = 0; i <= BoardHeight - WinCondition; i++)
        {
            for (int j = 0; j <= BoardWidth - WinCondition; j++)
            {
                bool winConditionMet = true;
                for (int k = 1; k < WinCondition; k++)
                {
                    if (Board[i, j] == 0 || Board[i, j] != Board[i + k, j + k])
                    {
                        winConditionMet = false;
                        break;
                    }
                }
                if (winConditionMet)
                {
                    Win = PlayerNumber;
                    return;
                }
            }
        }
        
        for (int i = 0; i <= BoardHeight - WinCondition; i++)
        {
            for (int j = WinCondition - 1; j < BoardWidth; j++)
            {
                bool winConditionMet = true;
                for (int k = 1; k < WinCondition; k++)
                {
                    if (Board[i, j] == 0 || Board[i, j] != Board[i + k, j - k])
                    {
                        winConditionMet = false;
                        break;
                    }
                }
                if (winConditionMet)
                {
                    Win = PlayerNumber;
                    return;
                }
            }
        }
    }
    
    private void CheckForAbilities()
    {
        if (Player1InitialChips - ChipsLeft[1] == AbilitiesAfterNMoves)
        {
            Player1Abilities = true;
        }

        if (Player2InitialChips - ChipsLeft[2] == AbilitiesAfterNMoves)
        {
            Player2Abilities = true;
        }
    }

    private bool IsInsideMovableBoard(int x, int y)
    {
        return x >= GridY && x < GridY + MovableBoardHeight && y >= GridX && y < GridX + MovableBoardWidth;
    }

    public Response PlaceChip(int x, int y)
    {
        bool isInsideMovableBoard = IsInsideMovableBoard(x, y);
    
        if (!isInsideMovableBoard)
        {
            return new Response(){ Success = false, Message = "You can only place chips inside the movable board area" };
        }

        if (Board[x, y] != 0)
        {
            return new Response(){ Success = false, Message = "You cannot place chip here" };
        }

        if (ChipsLeft[PlayerNumber] <= 0)
        {
            return new Response(){ Success = false, Message = "You have ran out of chips" };
        }
    
        Board[x, y] = PlayerNumber;
        CheckForWinner();
        ChipsLeft[PlayerNumber]--;
        CheckForAbilities();
        PlayerNumber = PlayerNumber == 1 ? 2 : 1;
        return new Response(){ Success = true, Message = "Chip placed" };
    }

    public Response MoveChip(int sourceX, int sourceY, int targetX, int targetY)
    {
        if (PlayerNumber == 1 && !Player1Abilities || PlayerNumber == 2 && !Player2Abilities)
        {
            return new Response(){ Success = false, Message = $"Place {AbilitiesAfterNMoves} to unlock abilities" };
        }
        
        if (Board[sourceX, sourceY] != PlayerNumber)
        {
            return new Response() { Success = false, Message = "You cannot move enemy's chips" };
        }
        
        bool isInsideMovableBoard = IsInsideMovableBoard(targetX, targetY);
        
        if (Board[targetX, targetY] != 0 || !isInsideMovableBoard)
        {
            return new Response() { Success = false, Message = "You cannot move chip there" };
        }
        
        Board[targetX, targetY] = PlayerNumber;
        Board[sourceX, sourceY] = 0;
        CheckForWinner();
        PlayerNumber = PlayerNumber == 1 ? 2 : 1;
        return new Response() { Success = true, Message = "Chip moved" };
    }
    
    public bool TakeOutChip(int x, int y)
    {
        if (Board[x, y] != 0)
        {
            Board[x, y] = 0;
            CheckForWinner();
            ChipsLeft[Board[x, y]]++;
            PlayerNumber = PlayerNumber == 1 ? 2 : 1;
            return true;
        }

        return false;
    }

    public Response MoveMovableBoard(string direction)
    {
        if (PlayerNumber == 1 && !Player1Abilities || PlayerNumber == 2 && !Player2Abilities)
        {
            return new Response(){ Success = false, Message = $"Place {AbilitiesAfterNMoves} to unlock abilities" };
        }
        
        int newGridX = GridX;
        int newGridY = GridY;
        
        switch (direction)
        {
            case "up":
                if (newGridY - 1 >= 0) newGridY--;
                break;
            case "down":
                if (newGridY + MovableBoardHeight < BoardHeight) newGridY++;
                break;
            case "left":
                if (newGridX - 1 >= 0) newGridX--;
                break;
            case "right":
                if (newGridX + MovableBoardWidth < BoardWidth) newGridX++;
                break;
            case "up-left":
                if (newGridY - 1 >= 0 && newGridX - 1 >= 0)
                {
                    newGridY--;
                    newGridX--;
                }
                break;
            case "up-right":
                if (newGridY - 1 >= 0 && newGridX + MovableBoardWidth < BoardWidth)
                {
                    newGridY--;
                    newGridX++;
                }
                break;
            case "down-left":
                if (newGridY + MovableBoardHeight < BoardHeight && newGridX - 1 >= 0)
                {
                    newGridY++;
                    newGridX--;
                }
                break;
            case "down-right":
                if (newGridY + MovableBoardHeight < BoardHeight && newGridX + MovableBoardWidth < BoardWidth)
                {
                    newGridY++;
                    newGridX++;
                }
                break;
            default:
                return new Response() { Success = false, Message = "You cannot move board there" };
        }
        
        GridX = newGridX;
        GridY = newGridY;
        
        PlayerNumber = PlayerNumber == 1 ? 2 : 1;
        
        return new Response() { Success = true, Message = "Board moved" };
    }

    public GameState SaveGame()
    {
        int rows = Board.GetLength(0);
        int cols = Board.GetLength(1);
        var jagged = new int[rows][];

        for (int i = 0; i < rows; i++)
        {
            jagged[i] = new int[cols];
            for (int j = 0; j < cols; j++)
            {
              jagged[i][j] = Board[i, j];
            }
        }
        
        var state = new GameState()
        {
            Board = jagged,
            GridX = GridX,
            GridY = GridY,
            ChipsLeft = ChipsLeft,
            PlayerNumber = PlayerNumber,
            Player1Abilities = Player1Abilities,
            Player2Abilities = Player2Abilities,
            Win = Win
        };

        return state;
    }
    
    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("Game Board:");
        for (int i = 0; i < BoardHeight; i++)
        {
            for (int j = 0; j < BoardWidth; j++)
            {
                sb.Append(Board[i, j] + " ");
            }
            sb.AppendLine();
        }

        sb.AppendLine("Movable Board:");
        for (int i = 0; i < MovableBoardHeight; i++)
        {
            for (int j = 0; j < MovableBoardWidth; j++)
            {
                sb.Append(MovableBoard[i, j] + " ");
            }
            sb.AppendLine();
        }

        sb.AppendLine($"Grid Position: ({GridX}, {GridY})");
        sb.AppendLine($"Chips Left: Player 1 = {ChipsLeft[0]}, Player 2 = {ChipsLeft[1]}");
        sb.AppendLine($"Current Player: {PlayerNumber}");
        sb.AppendLine($"Winner: {(Win == null ? "None" : Win == 0 ? "Draw" : $"Player {Win}")}");

        return sb.ToString();
    }
}
