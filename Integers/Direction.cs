namespace Galaxon.Numerics.Integers;

public enum EDirection
{
    Up,

    Right,

    Down,

    Left
}

public static class Direction
{
    public static EDirection TurnClockwise(EDirection currentDirection) =>
        currentDirection == EDirection.Left ? EDirection.Up : currentDirection + 1;

    public static EDirection TurnAntiClockwise(EDirection currentDirection) =>
        currentDirection == EDirection.Up ? EDirection.Left : currentDirection - 1;
}
