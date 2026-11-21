using Godot;
using Grid;

[GlobalClass]
public partial class Tile : Node2D, IClickable
{
	[Export] public Sprite2D graphics;
	[Export] public string tileType = "field";
	private bool isHighlighted;

    public override void _Ready()
    {
        WorldGridManager.MoveOwner(this, WorldGridManager.CalculateSnapPos(GlobalPosition));
    }


	public Sprite2D GetGraphics()
	{
		return graphics;
	}
	public void Highlight()
	{
		// Replace this later with an animation + add sound
		graphics.Scale = new Vector2(1.05f,1.05f);
		WorldGridManager.highlightedTile = this;
	}
	public void OnClickEvent()
	{
		// Open GUI Options for the selected tile
		graphics.Scale = new Vector2(1.15f,1.15f);
	}
	public void Reset()
	{
		// Replace this later with an animation + add sound
		graphics.Scale = new Vector2(1.0f,1.0f);
	}
}
