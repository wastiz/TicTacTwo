﻿namespace DAL;

using System.ComponentModel.DataAnnotations;

public class GameStateDB
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string GameConfigId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Board { get; set; } = default!;
    public string ChipsLeft { get; set; } = default!;
    public string PlayersMoves { get; set; } = default!;
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int PlayerNumber { get; set; } = default!;
    public bool Player1Options { get; set; } = default!;
    public bool Player2Options { get; set; } = default!;
    public int Win { get; set; } = default!;
}