using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Domain;

public class GameState
{
  [Key]
  public string Id { get; set; } = Guid.NewGuid().ToString();

  public string BoardJson { get; set; } = default!;
  public string ChipsLeftJson { get; set; } = default!;
  public string PlayersMovesJson { get; set; } = default!;

  [NotMapped]
  public int[][] Board
  {
    get => JsonSerializer.Deserialize<int[][]>(BoardJson)!;
    set => BoardJson = JsonSerializer.Serialize(value);
  }

  [NotMapped]
  public int[] ChipsLeft
  {
    get => JsonSerializer.Deserialize<int[]>(ChipsLeftJson)!;
    set => ChipsLeftJson = JsonSerializer.Serialize(value);
  }

  [NotMapped]
  public int[] PlayersMoves
  {
    get => JsonSerializer.Deserialize<int[]>(PlayersMovesJson)!;
    set => PlayersMovesJson = JsonSerializer.Serialize(value);
  }

  public int GridX { get; set; }
  public int GridY { get; set; }
  public int PlayerNumber { get; set; }
  public bool Player1Options { get; set; }
  public bool Player2Options { get; set; }
  public int Win { get; set; }
}
