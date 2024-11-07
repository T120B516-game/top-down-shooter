using Client.HubClients;
using Client.Observer;

namespace Client;

// Facade
public class ClientUpdateHandler
{
	private readonly IHubClient _mainHubClient;
	private readonly ContainerControl _containerControl;

	private readonly InputPublisher _inputPublisher = new();
	private readonly MovementHandler _movementHandler = new();
	private readonly CrosshairRenderer _crosshairRenderer = new();
	private readonly PauseHandler _pauseHandler = new();

	private readonly List<object> _components = [];

	public ClientUpdateHandler(ContainerControl containerControl, IHubClient mainHubClient)
	{
		_containerControl = containerControl;
		_mainHubClient = mainHubClient;

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

	private void HandleInput(InputEvent inputEvent)
	{
		_inputPublisher.AddInputEvent(inputEvent);
		_inputPublisher.NotifyObservers();
	}

	public async Task UpdateAsync()
	{
		if (_pauseHandler.IsPaused)
		{
			return;
		}

		foreach (var sendable in _components.OfType<ISendable>())
		{
			await sendable.SendAsync(_mainHubClient.Connection);
		}

		foreach (var updateable in _components.OfType<IUpdateable>())
		{
			updateable.Update();
		}
	}
}
