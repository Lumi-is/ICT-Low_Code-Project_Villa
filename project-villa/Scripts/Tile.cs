using Godot;

public partial class Tile : Node, IClickable
{
	[Export]
	public Node2D graphics;
	private bool isHighlighted;
	
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
}
