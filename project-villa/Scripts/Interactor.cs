using Godot;

public partial class Interactor : Node2D
{
	// Which *clickable* object is mouse currently hovering over, and is it clicked?
	IClickable clickable;
	Vector2 mousePos;

	public override void _PhysicsProcess(double delta)
	{
		var space = GetViewport().World2D.DirectSpaceState;
		var query = new PhysicsPointQueryParameters2D
		{
			CollideWithAreas = true,
			Position = mousePos
		};
		var result = space.IntersectPoint(query, 1);

		if (result.Count == 0)
		{
			clickable?.Reset();
			clickable = null;
		} else
		{
			IClickable castForm = (GodotObject)result[0]["collider"] as IClickable;
			clickable = castForm;
			clickable.Highlight();
			
			if(Input.IsActionJustPressed("Mouse1"))
			{
				// Testing purposes
				clickable.OnClickEvent();
			}
		}
	}

	public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();
	}
}
