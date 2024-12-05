using Backend;
using Client;
using Client.Observer;
using System.Windows.Forms;

namespace Tests.StressTest;

public class StressTests
{
	private readonly Startup backend = new();

	[Fact]
	public async Task TestAsync()
	{
		_ = backend.StartAsync();
		while (backend.RunningTask == null)
		{
			await Task.Delay(1);
		}

		var cts = new CancellationTokenSource();
		cts.CancelAfter(TimeSpan.FromMinutes(5));

		var tasks = new List<Task>();
		for (var i = 0; i < 1000; i++)
		{
			tasks.Add(SimulatePlayerAsync(cts.Token));
		}
		await Task.WhenAll(tasks);
	}

	public async Task SimulatePlayerAsync(CancellationToken ct)
	{
		var form = new Form1();
		form.CreateControl();

		_ = form.Handle;
		while (!form.IsHandleCreated)
		{
			await Task.Delay(1, ct);
		}

		form.Form1_Load(this, null);
		while (!form.IsReady)
		{
			await Task.Delay(1, ct);
		}

		var movements = new List<Keys>()
		{
			Keys.W, Keys.D, Keys.S, Keys.A
		};

		Keys? previousMovement = null;

		while (!ct.IsCancellationRequested)
		{
			foreach (var movement in movements)
			{
				Move(form, movement, previousMovement);
				previousMovement = movement;

				try
				{
					await Task.Delay(10, ct);
				}
				catch
				{
					return;
				}
			}
		}
	}

	private static void Move(Form1 form, Keys direction, Keys? previousDirection = null)
	{
		if (previousDirection.HasValue)
		{
			form.UpdateHandler.HandleInput(new InputEvent
			{
				KeyboardEvent = new KeyboardEvent(new KeyEventArgs(previousDirection.Value), KeyEventType.KeyUp)
			});
		}

		form.UpdateHandler.HandleInput(new InputEvent
		{
			KeyboardEvent = new KeyboardEvent(new KeyEventArgs(direction), KeyEventType.KeyDown)
		});
	}
}
