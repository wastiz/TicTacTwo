using System.ComponentModel.DataAnnotations;

namespace Shared.GameConfigDtos;

public class GameConfigDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required(ErrorMessage = "Configuration name is required")]
    [StringLength(50, ErrorMessage = "Name is too long (max 50 chars)")]
    public string Name { get; set; }
    
    [Required]
    [Range(3, 15, ErrorMessage = "Board width must be between 3 and 15")]
    public int BoardSizeWidth { get; set; }
    
    [Required]
    [Range(3, 15, ErrorMessage = "Board height must be between 3 and 15")]
    public int BoardSizeHeight { get; set; }
    
    [Required]
    [Range(1, 15, ErrorMessage = "Movable board width must be at least 1")]
    public int MovableBoardWidth { get; set; }
    
    [Required]
    [Range(1, 15, ErrorMessage = "Movable board height must be at least 1")]
    public int MovableBoardHeight { get; set; }
    
    [Required]
    [Range(1, 100, ErrorMessage = "Chips count must be at least 1")]
    public int Player1Chips { get; set; }
    
    [Required]
    [Range(1, 100, ErrorMessage = "Chips count must be at least 1")]
    public int Player2Chips { get; set; } = 4;
    
    [Required]
    [Range(1, 15, ErrorMessage = "Win condition must be between 1 and 15")]
    public int WinCondition { get; set; } // Pieces in row to win
    
    [Required]
    [Range(0, 50, ErrorMessage = "Options after N moves must be positive")]
    public int AbilitiesAfterNMoves { get; set; } // 0 to instantly unlock abilities
}