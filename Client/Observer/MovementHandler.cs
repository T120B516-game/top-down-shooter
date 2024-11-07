using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Concurrent;

namespace Client.Observer;

public enum Direction
{
    Up = 1,
    Right = 2,
    Down = 3,
    Left = 4,
    Undo = 5
}

public class MovementHandler : IInputObserver, ISendable
{
    private readonly ConcurrentDictionary<Direction, bool> _states = new()
    {
        [Direction.Up] = false,
        [Direction.Right] = false,
        [Direction.Down] = false,
        [Direction.Left] = false,
        [Direction.Undo] = false
    };

    public void Update(InputEvent input)
    {
        var keyboardEvent = input.KeyboardEvent;
        if (keyboardEvent == null)
        {
            return;
        }

        var direction = GetDirection(keyboardEvent.Args.KeyCode);
        if (direction == null)
        {
            return;
        }

        var isMoving = keyboardEvent.Type == KeyEventType.KeyDown;
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
