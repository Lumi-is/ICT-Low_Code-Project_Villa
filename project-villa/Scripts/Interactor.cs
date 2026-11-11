using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class Interactor : Node2D
{
	// Which *clickable* object is mouse currently hovering over, and is it clicked?
	IClickable clickable;
	IDraggable draggable;
	Vector2 mousePos;


	PhysicsDirectSpaceState2D space;
	PhysicsPointQueryParameters2D query;
	Array<Dictionary> results;
	List<GodotObject> colliders;

	public override void _PhysicsProcess(double delta)
	{
		// Create a new point raycast and store its results in a variable
		space = GetViewport().World2D.DirectSpaceState;
		query = new PhysicsPointQueryParameters2D
		{
			CollideWithAreas = true,
			Position = mousePos
		};
		results = space.IntersectPoint(query);


		// Create a list of interactor-compatible colliders from the results and store them in a seperate list
		colliders = new();
        foreach(Dictionary d in results)
        {
			var obj = (GodotObject)d["collider"];
			if(obj as CanvasItem != null)
			{
				colliders.Add(obj);
			}
        }

		// Sort the collider list
        for(int i = 0; i < colliders.Count-1;)
        {
			CanvasItem current = (CanvasItem)colliders[i];
			CanvasItem  next = (CanvasItem)colliders[i+1];
            if(next.ZIndex > current.ZIndex)
			{
                (colliders[i+1], colliders[i]) = (colliders[i], colliders[i+1]);
            } else
			{
				i++;
			}
        }
	}

	IDraggable cDraggable = null;
	IClickable cClickable = null;


	public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();

		cClickable?.Reset();

		if(cClickable != null && cDraggable == null)
		{
			ResetCanvasPriority(cClickable, cClickable.GetGraphics());
		}

		if(colliders.Count == 0) return;

		if(Input.IsActionJustPressed("Mouse1") && colliders[0] is IDraggable)
		{
			cDraggable = (IDraggable)colliders[0];
			SetPriority(cDraggable, cDraggable.GetGraphics(), 2);
		}

		if(Input.IsActionPressed("Mouse1") && cDraggable != null)
		{
			cDraggable.Drag(mousePos);
		}

		if(Input.IsActionJustReleased("Mouse1") && cDraggable != null)
		{
			cDraggable.Release();
			ResetCanvasPriority(cDraggable, cDraggable.GetGraphics());
			cDraggable = null;
		}

		if(colliders.Count == 1 && colliders[0] is IClickable && cDraggable == null)
		{
			cClickable = (IClickable)colliders[0];
			SetPriority(cClickable, cClickable.GetGraphics(),1);

			cClickable.Highlight();

			if(Input.IsActionJustPressed("Mouse2") )
			{
				cClickable.OnClickEvent();
			}
		}

	}

	private static void ResetCanvasPriority(object item, Sprite2D graphics)
	{
		if(item is CanvasItem canvas) 
		{
			canvas.ZIndex = 0;
			graphics.ZIndex = 0;
		}
	}

	private static void SetPriority(object item, Sprite2D graphics, int priority)
	{
		if(item is CanvasItem canvas) 
		{
			canvas.ZIndex = priority;
			graphics.ZIndex = priority;
		}
	}

	public static void SnapToMouse(Node2D someDraggable, Vector2 mousePos)
	{
		Node2D draggable = someDraggable.GetOwner<Node2D>();
		draggable.Position = mousePos;
	}
}