using Client.HubClients;

namespace Client.Mediator;

public abstract class BaseComponent
{
	public GameMediator Mediator { get; set; }
}

public class GameMediator
{
	private readonly AssetManager _assetManager;
	private readonly NetworkComponent _networkComponent;

	public GameMediator(AssetManager assetManager, NetworkComponent networkComponent)
	{
		_assetManager = assetManager;
		_networkComponent = networkComponent;

		_assetManager.Mediator = this;
		_networkComponent.Mediator = this;
	}

	public void Notify(object sender, string @event, object state)
	{
		if (@event == "RemoveEnemy")
		{
			_assetManager.RemoveEntity((int)state);
		}
		else if (@event == "SpawnBullet")
		{
			var stateArr = (object[])state;

			var playerId = (int)stateArr[0];
			var x = (int)stateArr[1];
			var y = (int)stateArr[2];
			var dir = (string)stateArr[3];
			_assetManager.SpawnBullet(playerId, x, y, dir);
		}
	}
}

public class AssetManager : BaseComponent
{
	public void RemoveEntity(int id)
	{
		foreach (var picture in Globals.Form.Controls.OfType<PictureBox>())
		{
			if (picture.Tag is int numericTag && numericTag == id)
			{
				picture.Dispose();
			}
		}
	}

	public void SpawnBullet(int playerId, int x, int y, string direction)
	{
		if (playerId != Globals.PersonalID)
		{
			_ = new Bullet(Globals.Form, playerId, x, y, direction);
		}
	}
}

public class NetworkComponent : BaseComponent
{
	private readonly IHubClient? _hubClient;

	public void OnRemoveEnemy(int id)
		=> Mediator!.Notify(this, "RemoveEnemy", id);

	public void OnSpawnBullet(int playerId, int x, int y, string direction)
		=> Mediator!.Notify(this, "SpawnBullet", new object[] { playerId, x, y, direction });
}