namespace Client.Observer;

public class InputPublisher
{

	private readonly List<IInputObserver> _observers = [];
	private readonly Queue<InputEvent> _inputEvents = [];

	public int ActiveObserversCount => _observers.Count;
	public int AvailableEventsCount => _inputEvents.Count;

	public void AddObserver(IInputObserver observer) =>
		_observers.Add(observer);

	public void RemoveObserver(IInputObserver observer) =>
		_observers.Remove(observer);

	public void NotifyObservers()
	{
		while (_inputEvents.Count > 0)
		{
			var inputEvent = _inputEvents.Dequeue();
			_observers.ForEach(x => x.Update(inputEvent));
		}
	}

	public void AddInputEvent(InputEvent input) =>
		_inputEvents.Enqueue(input);
}

public interface IInputObserver
{
	void Update(InputEvent input);
}

public class InputEvent
{
	public KeyboardEvent? KeyboardEvent { get; set; }
	public MouseEvent? MouseEvent { get; set; }
}

public enum KeyEventType
{
	KeyDown,
	KeyUp,
}
public record KeyboardEvent(KeyEventArgs Args, KeyEventType Type);


public enum MouseEventType
{
	Move,
}
public record MouseEvent(Point Location, MouseEventType Type);