namespace AstroMultimedia.Numerics.Integers;

public enum EDirection
{
    Up,
    Right,
    Down,
    Left
}

public static class Direction
{
    public static EDirection GoClockwise(EDirection currentDirection) =>
        currentDirection == EDirection.Left ? EDirection.Up : currentDirection + 1;

    public static EDirection GoAntiClockwise(EDirection currentDirection) =>
        currentDirection == EDirection.Up ? EDirection.Left : currentDirection - 1;
}
