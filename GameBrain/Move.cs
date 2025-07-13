

public class Move
{
    public enum MoveType { Place, MoveChip, MoveBoard }
    public MoveType Type { get; set; }
    public int X1 { get; set; }
    public int Y1 { get; set; }
    public int X2 { get; set; }
    public int Y2 { get; set; }
    public string Direction { get; set; }

    public static Move CreatePlaceMove(int x, int y)
    {
        return new Move { Type = MoveType.Place, X1 = x, Y1 = y };
    }

    public static Move CreateMoveChip(int fromX, int fromY, int toX, int toY)
    {
        return new Move { Type = MoveType.MoveChip, X1 = fromX, Y1 = fromY, X2 = toX, Y2 = toY };
    }

    public static Move CreateMoveBoard(string direction)
    {
        return new Move { Type = MoveType.MoveBoard, Direction = direction };
    }
}