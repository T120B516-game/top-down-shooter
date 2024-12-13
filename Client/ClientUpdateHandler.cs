﻿using Client.HubClients;
using Client.Observer;

namespace Client;

// Facade
public class ClientUpdateHandler
{
    private readonly INetworkHandler _networkHandler;
    private readonly ContainerControl _containerControl;

    private readonly InputPublisher _inputPublisher = new();
    private readonly MovementHandler _movementHandler = new();
    private readonly CrosshairRenderer _crosshairRenderer = new();
    private readonly PauseHandler _pauseHandler = new();

    private readonly List<object> _components = [];

    public ClientUpdateHandler(ContainerControl containerControl, INetworkHandler networkHandler)
    {
        _containerControl = containerControl;
        _networkHandler = networkHandler;

        _inputPublisher.AddObserver(_movementHandler);
        _inputPublisher.AddObserver(_crosshairRenderer);
        _inputPublisher.AddObserver(_pauseHandler);

        _crosshairRenderer.Init(_containerControl.Controls);
        _pauseHandler.Init(_containerControl.Controls);

        _components.Add(_inputPublisher);
        _components.Add(_movementHandler);
        _components.Add(_crosshairRenderer);
        _components.Add(_pauseHandler);
    }

    public void OnKeyDown(object? sender, KeyEventArgs e) =>
        HandleInput(new InputEvent()
        {
            KeyboardEvent = new(e, KeyEventType.KeyDown)
        });

    public void OnKeyUp(object? sender, KeyEventArgs e) =>
        HandleInput(new InputEvent()
        {
            KeyboardEvent = new(e, KeyEventType.KeyUp)
        });

    public void OnWindowsMessage(ref Message message)
    {
        const int WM_MOUSEMOVE = 0x0200;

        if (message.Msg == WM_MOUSEMOVE)
        {
            var clientCoords = _containerControl.PointToClient(Cursor.Position);

            HandleInput(new InputEvent()
            {
                MouseEvent = new(clientCoords, MouseEventType.Move)
            });
        }
    }

    /// <summary> Public only for testing. </summary>
    public void HandleInput(InputEvent inputEvent)
    {
        _inputPublisher.AddInputEvent(inputEvent);
        _inputPublisher.NotifyObservers();
    }

    public async Task<bool> UpdateAsync()
    {
        if (_pauseHandler.IsPaused)
        {
            return false;
        }

        await _movementHandler.SendAsync(_networkHandler);

        foreach (var updateable in _components.OfType<IUpdateable>())
        {
            updateable.Update();
        }

        return true;
    }
    // Add method to handle teleportation commands
    public async Task HandleTeleportationAsync(int x, int y)
    {
        // Send teleport coordinates to the server via the network handler
        await _networkHandler.SendTeleportAsync(x, y, Globals.PersonalID);
        Console.WriteLine($"Teleporting player to ({x}, {y})...");
    }

    public async Task HandleEnemyDamageAsync(int id)
    {
	    await _networkHandler.DamageEnemy(id);
    }

    public async Task HandleBulletFired(int id)
    {
        await _networkHandler.BulletFired(id);
    }
}
