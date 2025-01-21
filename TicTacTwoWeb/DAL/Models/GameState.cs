using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DAL;


public class GameState
{
    [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BoardJson { get; set; } = default!;
    public string ChipsLeftJson { get; set; } = default!;
    public string PlayersMovesJson { get; set; } = default!;

    [NotMapped]
    public int[,] Board
    {
        get => DeserializeBoard(BoardJson);
        set => BoardJson = SerializeBoard(value);
    }

    [NotMapped]
    public int[] ChipsLeft
    {
        get => JsonSerializer.Deserialize<int[]>(ChipsLeftJson);
        set => ChipsLeftJson = JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public int[] PlayersMoves
    {
        get => JsonSerializer.Deserialize<int[]>(PlayersMovesJson);
        set => PlayersMovesJson = JsonSerializer.Serialize(value);
    }

    public int GridX { get; set; }
    public int GridY { get; set; }
    public int PlayerNumber { get; set; } = default!;
    public bool Player1Options { get; set; } = default!;
    public bool Player2Options { get; set; } = default!;
    public int Win { get; set; } = default!;

    private string SerializeBoard(int[,] board)
    {
        var rows = board.GetLength(0);
        var cols = board.GetLength(1);
        var flatArray = new int[rows * cols];
        Buffer.BlockCopy(board, 0, flatArray, 0, flatArray.Length * sizeof(int));
        var boardData = new { Rows = rows, Cols = cols, Data = flatArray };
        return JsonSerializer.Serialize(boardData);
    }

    private int[,] DeserializeBoard(string boardJson)
    {
        var boardData = JsonSerializer.Deserialize<BoardData>(boardJson);
        if (boardData == null)
        {
            throw new InvalidOperationException("Invalid board JSON data.");
        }
        var board = new int[boardData.Rows, boardData.Cols];
        Buffer.BlockCopy(boardData.Data, 0, board, 0, boardData.Data.Length * sizeof(int));
        return board;
    }

    private class BoardData
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int[] Data { get; set; } = default!;
    }
}
