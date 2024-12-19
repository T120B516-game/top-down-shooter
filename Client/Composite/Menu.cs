namespace Client.Composite;

public interface IMenuComponent
{
	ToolStripItem CreateMenuItem();
}

public class MenuItemLeaf : IMenuComponent
{
	private readonly string _name;
	private readonly EventHandler _action;

	public MenuItemLeaf(string name, EventHandler action)
	{
		_name = name;
		_action = action;
	}

	public ToolStripItem CreateMenuItem()
	{
		var toReturn = new ToolStripMenuItem(_name);
		toReturn.Click += _action;
		
		return toReturn;
	}
}

public class MenuComposite : IMenuComponent
{
	private readonly string _name;
	private readonly List<IMenuComponent> _menuComponents = [];

	public MenuComposite(string name)
	{
		_name = name;
	}

	public void Add(IMenuComponent component)
	{
		_menuComponents.Add(component);
	}

	public void Remove(IMenuComponent component)
	{
		_menuComponents.Remove(component);
	}

	public ToolStripItem CreateMenuItem()
	{
		var menuItem = new ToolStripMenuItem(_name);
		foreach (var component in _menuComponents)
		{
			var nested = component.CreateMenuItem();
			menuItem.DropDownItems.Add(nested);
		}

		return menuItem;
	}
}