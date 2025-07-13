using Domain;
using Shared;

public class Bot
{
    private int _botPlayerNumber;
    private int _maxDepth;

    public Bot(int botPlayerNumber, int maxDepth = 3)
    {
        _botPlayerNumber = botPlayerNumber;
        _maxDepth = maxDepth;
    }

    public Move GetBestMove(GameBrain brain)
    {
        if (brain.PlayerNumber != _botPlayerNumber)
            throw new Exception("Not bot's turn");

        List<Move> possibleMoves = GenerateMoves(brain);
        if (possibleMoves.Count == 0) return null;

        Move bestMove = null;
        int bestScore = int.MinValue;
        GameState initialState = brain.SaveGame();

        foreach (Move move in possibleMoves)
        {
            Response response = ApplyMove(brain, move);
            if (!response.Success) continue;

            int score = Minimax(brain, _maxDepth - 1, int.MinValue, int.MaxValue, false);
            brain.LoadState(initialState);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Minimax(GameBrain brain, int depth, int alpha, int beta, bool isMaximizing)
    {
        if (brain.Win != 0 || depth == 0)
            return Evaluate(brain);

        List<Move> moves = GenerateMoves(brain);
        GameState stateBefore = brain.SaveGame();
        int initialPlayer = brain.PlayerNumber;

        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            foreach (Move move in moves)
            {
                Response response = ApplyMove(brain, move);
                if (!response.Success) continue;

                int eval = Minimax(brain, depth - 1, alpha, beta, false);
                brain.LoadState(stateBefore);
                brain.PlayerNumber = initialPlayer;

                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha) break;
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            foreach (Move move in moves)
            {
                Response response = ApplyMove(brain, move);
                if (!response.Success) continue;

                int eval = Minimax(brain, depth - 1, alpha, beta, true);
                brain.LoadState(stateBefore);
                brain.PlayerNumber = initialPlayer;

                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha) break;
            }
            return minEval;
        }
    }

    private List<Move> GenerateMoves(GameBrain brain)
    {
        List<Move> moves = new List<Move>();
        bool abilitiesAvailable = (brain.PlayerNumber == 1 && brain.Player1Abilities) || 
                                 (brain.PlayerNumber == 2 && brain.Player2Abilities);

        // Place chip moves
        for (int x = brain.GridY; x < brain.GridY + brain.MovableBoardHeight; x++)
        {
            for (int y = brain.GridX; y < brain.GridX + brain.MovableBoardWidth; y++)
            {
                if (brain.Board[x, y] == 0)
                    moves.Add(Move.CreatePlaceMove(x, y));
            }
        }

        if (!abilitiesAvailable) return moves;

        // Move chip moves
        for (int x = brain.GridY; x < brain.GridY + brain.MovableBoardHeight; x++)
        {
            for (int y = brain.GridX; y < brain.GridX + brain.MovableBoardWidth; y++)
            {
                if (brain.Board[x, y] == brain.PlayerNumber)
                {
                    for (int nx = brain.GridY; nx < brain.GridY + brain.MovableBoardHeight; nx++)
                    {
                        for (int ny = brain.GridX; ny < brain.GridX + brain.MovableBoardWidth; ny++)
                        {
                            if (brain.Board[nx, ny] == 0)
                                moves.Add(Move.CreateMoveChip(x, y, nx, ny));
                        }
                    }
                }
            }
        }

        // Move board moves
        string[] directions = { "up", "down", "left", "right", "up-left", "up-right", "down-left", "down-right" };
        foreach (string dir in directions)
        {
            moves.Add(Move.CreateMoveBoard(dir));
        }

        return moves;
    }

    private Response ApplyMove(GameBrain brain, Move move)
    {
        switch (move.Type)
        {
            case Move.MoveType.Place:
                return brain.PlaceChip(move.X1, move.Y1);
            case Move.MoveType.MoveChip:
                return brain.MoveChip(move.X1, move.Y1, move.X2, move.Y2);
            case Move.MoveType.MoveBoard:
                return brain.MoveMovableBoard(move.Direction);
            default:
                return new Response { Success = false };
        }
    }

    private int Evaluate(GameBrain brain)
    {
        if (brain.Win == _botPlayerNumber) return 10000;
        if (brain.Win == 3) return 0; // Draw
        if (brain.Win != 0) return -10000;

        int score = 0;
        int opponent = _botPlayerNumber == 1 ? 2 : 1;

        // Horizontal lines
        for (int i = 0; i < brain.BoardHeight; i++)
        {
            for (int j = 0; j <= brain.BoardWidth - brain.WinCondition; j++)
            {
                score += EvaluateLine(brain, i, j, 0, 1, _botPlayerNumber);
                score -= EvaluateLine(brain, i, j, 0, 1, opponent);
            }
        }

        // Vertical lines
        for (int j = 0; j < brain.BoardWidth; j++)
        {
            for (int i = 0; i <= brain.BoardHeight - brain.WinCondition; i++)
            {
                score += EvaluateLine(brain, i, j, 1, 0, _botPlayerNumber);
                score -= EvaluateLine(brain, i, j, 1, 0, opponent);
            }
        }

        // Diagonal down-right
        for (int i = 0; i <= brain.BoardHeight - brain.WinCondition; i++)
        {
            for (int j = 0; j <= brain.BoardWidth - brain.WinCondition; j++)
            {
                score += EvaluateLine(brain, i, j, 1, 1, _botPlayerNumber);
                score -= EvaluateLine(brain, i, j, 1, 1, opponent);
            }
        }

        // Diagonal down-left
        for (int i = 0; i <= brain.BoardHeight - brain.WinCondition; i++)
        {
            for (int j = brain.WinCondition - 1; j < brain.BoardWidth; j++)
            {
                score += EvaluateLine(brain, i, j, 1, -1, _botPlayerNumber);
                score -= EvaluateLine(brain, i, j, 1, -1, opponent);
            }
        }

        // Center control bonus
        int centerX = brain.BoardWidth / 2;
        int centerY = brain.BoardHeight / 2;
        if (brain.Board[centerY, centerX] == _botPlayerNumber) score += 10;
        else if (brain.Board[centerY, centerX] == opponent) score -= 10;

        return score;
    }

    private int EvaluateLine(GameBrain brain, int startY, int startX, int dy, int dx, int player)
    {
        int playerCount = 0;
        int emptyCount = 0;

        for (int i = 0; i < brain.WinCondition; i++)
        {
            int y = startY + i * dy;
            int x = startX + i * dx;
            int cell = brain.Board[y, x];

            if (cell == player) playerCount++;
            else if (cell == 0) emptyCount++;
            else return 0; // Opponent's chip in line
        }

        // Weight by number of player's chips in the line
        return (int)Math.Pow(10, playerCount - 1) * (emptyCount > 0 ? 1 : 0);
    }
}