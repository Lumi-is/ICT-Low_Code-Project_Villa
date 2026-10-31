using Godot;
using System;

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
	public void StopHighlight()
	{
		// Replace this later with an animation + add sound
		graphics.Scale = new Vector2(1.0f,1.0f);
	}
	public void OnClickEvent()
	{
		// Open GUI Options for the selected tile
	}
	
	public void OnMouseEntered()
	{
		// What happens when mouse enters the tile
		Highlight();
	}
	public void OnMouseExited()
	{
		// What happens when mouse exits the tile
		StopHighlight();
	}
}
