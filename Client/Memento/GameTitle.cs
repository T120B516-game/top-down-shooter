namespace Client.Memento;

public class GameTitle
{
	public string Title { get; private set; } = "Form1";

	public GameTitleMemento Save()
	{
		return new GameTitleMemento(Title);
	}

	public void Restore(GameTitleMemento meme)
	{
		SetTitle(meme.GetState());
	}

	public void SetTitle(string title)
	{
		Title = title;
		Globals.Form.Text = Title;
	}
}

public class GameTitleMemento
{
	private readonly string _title;

	public GameTitleMemento(string title)
	{
		_title = title;
	}

	public string GetState()
	{
		return _title;
	}
}

public class GameTitleHandler
{
	private readonly List<GameTitleMemento> _memes = [];
	private readonly GameTitle _gameTitle;

	public GameTitleHandler(GameTitle gameTitle)
	{
		_gameTitle = gameTitle;
	}

	public void Backup()
	{
		var meme = _gameTitle.Save();
		_memes.Add(meme);
	}

	public void Undo()
	{
		if (_memes.Count == 0)
		{
			return;
		}

		var meme = _memes.Last();
		_memes.Remove(meme);

		_gameTitle.Restore(meme);
	}
}