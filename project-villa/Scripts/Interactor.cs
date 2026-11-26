using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Grid;
using ProjectVilla.Cards;

public partial class Interactor : Node2D
{
	public static Interactor Instance { get; private set; }

	private Vector2 mousePos;
	private PhysicsDirectSpaceState2D space;
	private PhysicsPointQueryParameters2D query;
	private Array<Dictionary> results;
	private List<GodotObject> colliders = new();

	private IDraggable cDraggable;
	private IClickable cClickable;
	private CardView _draggedCard;
	private Vector2 _dragOffset;
	private Tile _dragHighlightedTile;

	public override void _Ready()
	{
		Instance = this;
		WorldGridManager.Reset();
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		mousePos = GetGlobalMousePosition();
		space = GetViewport().World2D.DirectSpaceState;
		query = new PhysicsPointQueryParameters2D
		{
			CollideWithAreas = true,
			Position = mousePos
		};
		results = space.IntersectPoint(query);

		colliders = new();
		foreach (Dictionary result in results)
		{
			GodotObject obj = (GodotObject)result["collider"];

			if (obj is CanvasItem)
			{
				colliders.Add(obj);
			}
		}

		for (int i = 0; i < colliders.Count - 1;)
		{
			CanvasItem current = (CanvasItem)colliders[i];
			CanvasItem next = (CanvasItem)colliders[i + 1];

			if (next.ZIndex > current.ZIndex)
			{
				(colliders[i + 1], colliders[i]) = (colliders[i], colliders[i + 1]);
			}
			else
			{
				i++;
			}
		}
	}

	public override void _Process(double delta)
	{
		mousePos = GetGlobalMousePosition();

		if (_draggedCard != null)
		{
			UpdateCardDragHighlight();
			return;
		}

		cClickable?.Reset();

		if (cClickable != null && cDraggable == null)
		{
			ResetCanvasPriority(cClickable, cClickable.GetGraphics());
		}

		if (colliders.Count == 0)
		{
			WorldGridManager.highlightedTile = null;
			return;
		}

		if (Input.IsActionJustPressed("Mouse1") && colliders[0] is IDraggable draggable)
		{
			cDraggable = draggable;
			SetCanvasPriority(draggable, draggable.GetGraphics(), 2);
		}

		if (Input.IsActionPressed("Mouse1") && cDraggable != null)
		{
			cDraggable.Drag(mousePos);
		}

		if (Input.IsActionJustReleased("Mouse1") && cDraggable != null)
		{
			cDraggable.Release();
			ResetCanvasPriority(cDraggable, cDraggable.GetGraphics());
			cDraggable = null;
		}

		if (colliders.Count == 1 && colliders[0] is IClickable clickable && cDraggable == null)
		{
			cClickable = clickable;
			SetCanvasPriority(clickable, clickable.GetGraphics(), 1);
			clickable.Highlight();

			if (Input.IsActionJustPressed("Mouse2"))
			{
				clickable.OnClickEvent();
			}
		}
	}

	public void BeginCardDrag(CardView view, Vector2 screenPosition)
	{
		if (view == null)
		{
			return;
		}

		_draggedCard = view;
		_dragOffset = screenPosition - view.GlobalPosition;
		view.BeginDrag();
	}

	public void MoveCardDrag(Vector2 screenPosition)
	{
		if (_draggedCard == null)
		{
			return;
		}

		_draggedCard.MoveDrag(screenPosition, _dragOffset);
	}

	public void EndCardDrag()
	{
		if (_draggedCard == null)
		{
			return;
		}

		CardView view = _draggedCard;
		_draggedCard = null;

		Vector2 dropPosition = GetGlobalMousePosition();
		ClearCardDragHighlight();
		view.EndDrag();
		view.Hand?.PlayCard(view, dropPosition);
	}

	public void CancelCardDrag()
	{
		if (_draggedCard == null)
		{
			return;
		}

		_draggedCard.EndDrag();
		_draggedCard = null;
		ClearCardDragHighlight();
	}

	private void UpdateCardDragHighlight()
	{
		Tile tile = WorldGridManager.GetTileAtWorldPosition(mousePos);

		if (tile == _dragHighlightedTile)
		{
			return;
		}

		_dragHighlightedTile?.Reset();
		_dragHighlightedTile = tile;

		if (tile != null)
		{
			tile.Highlight();
		}
		else
		{
			WorldGridManager.highlightedTile = null;
		}
	}

	private void ClearCardDragHighlight()
	{
		if (_dragHighlightedTile != null)
		{
			_dragHighlightedTile.Reset();
			_dragHighlightedTile = null;
		}

		WorldGridManager.highlightedTile = null;
	}

	private static void ResetCanvasPriority(object item, Sprite2D graphics)
	{
		if (item is CanvasItem canvas)
		{
			canvas.ZIndex = 0;
			graphics.ZIndex = 0;
		}
	}

	private static void SetCanvasPriority(object item, Sprite2D graphics, int priority)
	{
		if (item is CanvasItem canvas)
		{
			canvas.ZIndex = priority;
			graphics.ZIndex = priority;
		}
	}

	public static void SnapToMouse(Node2D someDraggable, Vector2 mousePos)
	{
		Node2D draggable = someDraggable.GetOwner<Node2D>() ?? someDraggable;
		draggable.Position = mousePos;
	}
}
