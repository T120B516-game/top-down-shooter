namespace Backend;

public class Program
{
	public static async Task Main() =>
		await new Startup().StartAsync();
}
