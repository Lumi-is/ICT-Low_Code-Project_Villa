using Godot;

public partial class Interactor : Node2D
{
	// Which *clickable* object is mouse currently hovering over, and is it clicked?
	IClickable clickable;
	IDraggable draggable;
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
			IClickable clickableCastForm = (GodotObject)result[0]["collider"] as IClickable;

			if(clickableCastForm != null)
			{
				clickable = clickableCastForm;
				clickable.Highlight();
				
				if(Input.IsActionJustPressed("Mouse2"))
				{
					// Testing purposes
					clickable.OnClickEvent();
				}
			}

			IDraggable draggableCastForm = (GodotObject)result[0]["collider"] as IDraggable;

			if(draggableCastForm != null)
			{
				draggable = draggableCastForm;
				
				
				if(Input.IsActionPressed("Mouse1"))
				{
					// Testing purposes
					draggable.Drag(mousePos);
				}

				if (Input.IsActionJustReleased("Mouse1"))
				{
					draggable.Release();
				}
			}
			
		}
	}

	public override void _Process(double delta)
	{
		
		mousePos = GetGlobalMousePosition();
	}

	public static void SnapToMouse(Node2D someDraggable, Vector2 mousePos)
	{
		Node2D draggable = someDraggable.GetOwner<Node2D>();
		draggable.Position = mousePos;
	}
}
