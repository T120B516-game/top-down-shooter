using Client.Observer;

namespace Tests.Observer;

public class InputPublisherTests
{
	[Fact]
	public void Should_Add_Observer()
	{
		var publisher = new InputPublisher();
		var observer = new RandomObserver();

		var countBefore = publisher.ActiveObserversCount;
		publisher.AddObserver(observer);
		var countAfter = publisher.ActiveObserversCount;

		Assert.True(countAfter == countBefore + 1);
	}

	[Fact]
	public void Should_Remove_Observer()
	{
		var publisher = new InputPublisher();
		var observer = new RandomObserver();
		publisher.AddObserver(observer);

		var countBefore = publisher.ActiveObserversCount;
		publisher.RemoveObserver(observer);
		var countAfter = publisher.ActiveObserversCount;

		Assert.True(countAfter == countBefore - 1);
	}

	[Fact]
	public void Should_Add_InputEvent()
	{
		var publisher = new InputPublisher();
		var inputEvent = new InputEvent();
		publisher.AddInputEvent(inputEvent);

		var countBefore = publisher.AvailableEventsCount;
		publisher.AddInputEvent(inputEvent);
		var countAfter = publisher.AvailableEventsCount;

		Assert.True(countAfter == countBefore + 1);
	}

	[Fact]
	public void Should_Notify_Observers()
	{
		var publisher = new InputPublisher();
		var observer = new RandomObserver();
		var inputEvent = new InputEvent();
		publisher.AddObserver(observer);
		publisher.AddInputEvent(inputEvent);

		publisher.NotifyObservers();

		Assert.True(observer.WasNotified);
	}
}

public class RandomObserver : IInputObserver
{
	public bool WasNotified { get; private set; }

	public void Update(InputEvent input)
	{
		WasNotified = true;
	}
}
