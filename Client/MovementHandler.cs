using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Concurrent;

namespace Client;

public enum Direction
{
    Up = 1,
    Right = 2,
    Down = 3,
    Left = 4,
    Undo = 5
}

public enum KeyEventType
{
    KeyDown,
    KeyUp,
}

public class MovementHandler
{
    private readonly ConcurrentDictionary<Direction, bool> _states = new()
    {
        [Direction.Up] = false,
        [Direction.Right] = false,
        [Direction.Down] = false,
        [Direction.Left] = false,
        [Direction.Undo] = false
    };

    public void ConsumeKeyEvent(KeyEventArgs e, KeyEventType keyEventType)
    {
        var direction = GetDirection(e.KeyCode);
        if (direction == null)
        {
            return;
        }

        var isMoving = keyEventType == KeyEventType.KeyDown;
        _states[direction.Value] = isMoving;
    }

    public async Task SendAsync(HubConnection connection)
    {
        foreach (var (direction, isMoving) in _states)
        {
            if (isMoving)
            {
                await connection.SendAsync("movePlayer", (int)direction, Globals.PersonalID);
            }
        }
    }

    private static Direction? GetDirection(Keys keyCode)
    {
        return keyCode switch
        {
            Keys.Up => Direction.Up,
            Keys.Right => Direction.Right,
            Keys.Down => Direction.Down,
            Keys.Left => Direction.Left,
            Keys.Z => Direction.Undo,
            _ => null
        };
    }
}
