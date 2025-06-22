using DAL;

public class GameBrain
{
    public int[,] board;
    public int boardWidth;
    public int boardHeight;
    public int[,] movableBoard;
    public int movableBoardWidth;
    public int movableBoardHeight;
    public int gridX;
    public int gridY;
    public int[] chipsLeft; //chipsLeft[1] - player 1 chips, chipsLeft[2] - player 2 chips
    public int[] playersMoves;
    public int playerNumber;
    public int chipsToOptions; // How many chips have player to place to get options
    public bool player1Options; // player 1 have more options
    public bool player2Options; // player 2 have more options
    public GameConfiguration gameConfig;
    private SessionRepository _sessionRepository = new SessionRepository();
    public int WinCondition;
    public int win = 0; //0 - nothing, 1 - player 1 won, 2 - player 2 won, 3 - draw
    
    
    /*public Brain(GameConfiguration config)
    {
        gameConfig = config;
        board = new int[config.BoardSizeHeight, config.BoardSizeWidth];
        boardWidth = config.BoardSizeWidth;
        boardHeight = config.BoardSizeHeight;
        movableBoard = new int[config.MovableBoardHeight, config.MovableBoardWidth];
        movableBoardWidth = config.MovableBoardWidth;
        movableBoardHeight = config.MovableBoardHeight;
        gridX = (board.GetLength(1) - movableBoard.GetLength(1)) / 2;
        gridY = (board.GetLength(0) - movableBoard.GetLength(0)) / 2;
        playerNumber = 1;
        playersMoves = [0, 0, 0];
        player1Options = false;
        player2Options = false;
        chipsLeft = config.ChipsCount;
        chipsToOptions = config.OptionsAfterNMoves; 
        WinCondition = config.WinCondition;
        win = 0;
    }*/
    
    
    
    public GameBrain(GameConfiguration config, GameState state)
    {
        gameConfig = config;
        board = new int[config.BoardSizeHeight, config.BoardSizeWidth];
        boardWidth = config.BoardSizeWidth;
        boardHeight = config.BoardSizeHeight;
        movableBoard = new int[config.MovableBoardHeight, config.MovableBoardWidth];
        movableBoardWidth = config.MovableBoardWidth;
        movableBoardHeight = config.MovableBoardHeight;
        WinCondition = config.WinCondition;
        chipsToOptions = config.OptionsAfterNMoves;
        
        board = state.Board;
        gridX = state.GridX;
        gridY = state.GridY;
        playerNumber = state.PlayerNumber;
        player1Options = state.Player1Options;
        player2Options = state.Player2Options;
        chipsLeft = state.ChipsLeft;
        playersMoves = state.PlayersMoves;
        win = state.Win;
    }
    
    public void CheckForWinner()
    {
        
        if (WinCondition > boardHeight || WinCondition > boardWidth)
        {
            return;
        }
        
        for (int i = 0; i < boardHeight; i++)
        {
            for (int j = 0; j <= boardWidth - WinCondition; j++)
            {
                bool winConditionMet = true;
                for (int k = 1; k < WinCondition; k++)
                {
                    if (board[i, j] == 0 || board[i, j] != board[i, j + k])
                    {
                        winConditionMet = false;
                        break;
                    }
                }
                if (winConditionMet)
                {
                    win = playerNumber;
                    return;
                }
            }
        }
        
        for (int j = 0; j < boardWidth; j++)
        {
            for (int i = 0; i <= boardHeight - WinCondition; i++)
            {
                bool winConditionMet = true;
                for (int k = 1; k < WinCondition; k++)
                {
                    if (board[i, j] == 0 || board[i, j] != board[i + k, j])
                    {
                        winConditionMet = false;
                        break;
                    }
                }
                if (winConditionMet)
                {
                    win = playerNumber;
                    return;
                }
            }
        }
        
        for (int i = 0; i <= boardHeight - WinCondition; i++)
        {
            for (int j = 0; j <= boardWidth - WinCondition; j++)
            {
                bool winConditionMet = true;
                for (int k = 1; k < WinCondition; k++)
                {
                    if (board[i, j] == 0 || board[i, j] != board[i + k, j + k])
                    {
                        winConditionMet = false;
                        break;
                    }
                }
                if (winConditionMet)
                {
                    win = playerNumber;
                    return;
                }
            }
        }
        
        for (int i = 0; i <= boardHeight - WinCondition; i++)
        {
            for (int j = WinCondition - 1; j < boardWidth; j++)
            {
                bool winConditionMet = true;
                for (int k = 1; k < WinCondition; k++)
                {
                    if (board[i, j] == 0 || board[i, j] != board[i + k, j - k])
                    {
                        winConditionMet = false;
                        break;
                    }
                }
                if (winConditionMet)
                {
                    win = playerNumber;
                    return;
                }
            }
        }
    }
    
    public void CheckForOptions()
    {
        if (playersMoves[1] == chipsToOptions)
        {
            player1Options = true;
        }

        if (playersMoves[2] == chipsToOptions)
        {
            player2Options = true;
        }
    }

    public bool placeChip(int x, int y)
    {
        if (board[x, y] == 0 && chipsLeft[playerNumber] > 0)
        {
            board[x, y] = playerNumber;
            CheckForWinner();
            chipsLeft[playerNumber]--;
            CheckForOptions();
            playersMoves[playerNumber] += 1;
            playerNumber = playerNumber == 1 ? 2 : 1;
            return true;
        }
        return false;
    }

    public bool moveChip(int sourceX, int sourceY, int targetX, int targetY)
    {
        if (board[sourceX, sourceY] == playerNumber && board[targetX, targetY] == 0)
        {
            board[targetX, targetY] = playerNumber;
            board[sourceX, sourceY] = 0;
            CheckForWinner();
            playersMoves[playerNumber] += 1;
            playerNumber = playerNumber == 1 ? 2 : 1;
            return true;
        }
        return false;
    }
    
    public bool takeOutChip(int x, int y)
    {
        if (board[x, y] != 0)
        {
            board[x, y] = 0;
            CheckForWinner();
            chipsLeft[board[x, y]]++;
            playersMoves[playerNumber] += 1;
            playerNumber = playerNumber == 1 ? 2 : 1;
            return true;
        }

        return false;
    }

    public bool moveMovableBoard(string direction)
    {
        int newGridX = gridX;
        int newGridY = gridY;
        
        switch (direction)
        {
            case "up":
                if (newGridY - 1 >= 0) newGridY--;
                break;
            case "down":
                if (newGridY + movableBoardHeight < boardHeight) newGridY++;
                break;
            case "left":
                if (newGridX - 1 >= 0) newGridX--;
                break;
            case "right":
                if (newGridX + movableBoardWidth < boardWidth) newGridX++;
                break;
            case "up-left":
                if (newGridY - 1 >= 0 && newGridX - 1 >= 0)
                {
                    newGridY--;
                    newGridX--;
                }
                break;
            case "up-right":
                if (newGridY - 1 >= 0 && newGridX + movableBoardWidth < boardWidth)
                {
                    newGridY--;
                    newGridX++;
                }
                break;
            case "down-left":
                if (newGridY + movableBoardHeight < boardHeight && newGridX - 1 >= 0)
                {
                    newGridY++;
                    newGridX--;
                }
                break;
            case "down-right":
                if (newGridY + movableBoardHeight < boardHeight && newGridX + movableBoardWidth < boardWidth)
                {
                    newGridY++;
                    newGridX++;
                }
                break;
            default:
                return false;
        }
        
        gridX = newGridX;
        gridY = newGridY;
        
        playersMoves[playerNumber] += 1;
        playerNumber = playerNumber == 1 ? 2 : 1;
        
        return true;
    }

    public void SaveGame(string sessionId)
    {
        var state = new GameState()
        {
            Board = board,
            GridX = gridX,
            GridY = gridY,
            ChipsLeft = chipsLeft,
            PlayerNumber = playerNumber,
            PlayersMoves = playersMoves,
            Player1Options = player1Options,
            Player2Options = player2Options,
            Win = win
        };
        _sessionRepository.SaveGameState(state, sessionId);
    }
    
    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("Game Board:");
        for (int i = 0; i < boardHeight; i++)
        {
            for (int j = 0; j < boardWidth; j++)
            {
                sb.Append(board[i, j] + " ");
            }
            sb.AppendLine();
        }

        sb.AppendLine("Movable Board:");
        for (int i = 0; i < movableBoardHeight; i++)
        {
            for (int j = 0; j < movableBoardWidth; j++)
            {
                sb.Append(movableBoard[i, j] + " ");
            }
            sb.AppendLine();
        }

        sb.AppendLine($"Grid Position: ({gridX}, {gridY})");
        sb.AppendLine($"Chips Left: Player 1 = {chipsLeft[0]}, Player 2 = {chipsLeft[1]}");
        sb.AppendLine($"Current Player: {playerNumber}");
        sb.AppendLine($"Winner: {(win == null ? "None" : win == 0 ? "Draw" : $"Player {win}")}");

        return sb.ToString();
    }
}