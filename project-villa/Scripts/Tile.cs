using Godot;

public partial class Tile : Node2D, IClickable, IDraggable
{
	[Export] public Node2D graphics;
	[Export] public string tileType = "field";
	private bool isHighlighted;

	public Sprite2D GetGraphics()
	{
		return (Sprite2D)graphics;
	}
	
	public void Highlight()
	{
		// Replace this later with an animation + add sound
		graphics.Scale = new Vector2(1.2f,1.2f);
	}
	public void Reset()
	{
		// Replace this later with an animation + add sound
		graphics.Scale = new Vector2(1.0f,1.0f);
	}
	public void OnClickEvent()
	{
		// Open GUI Options for the selected tile
		graphics.Scale = new Vector2(1.5f,1.5f);
	}

	public void Drag(Vector2 mousePos)
	{
		Interactor.SnapToMouse(this, mousePos);
	}

	public void Release()
	{
		Grid.WorldGridManager.MoveOwner(this, Grid.WorldGridManager.CalculateSnapPos(GlobalPosition));
	}
}
