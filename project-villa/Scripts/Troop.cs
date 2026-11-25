using Godot;
using Grid;

public partial class Troop : Node2D, IClickable, IDraggable
{
	[Export] public string troopType;
	[Export] public Sprite2D graphics;

	public bool[][] viableLocations = new bool[5][];
	private Vector2? dragStartLocation = null;
	const bool startPos = false;

	public bool hasMoved;
	public void OnClickEvent()
	{
		
	}

	public Sprite2D GetGraphics()
	{
		return graphics;
	}

	public void Highlight()
	{
		
	}

	public void Reset()
	{
		
	}
	public override void _Ready()
	{
		TurnManager.troopList.Add(this);
		
		if (troopType == "rook")
		{
			// Moves like a rook from chess
			bool[] row0 = {false, false, true, false, false};
			bool[] row1 = {false, false, true, false, false};
			bool[] row2 = {true, true, startPos, true, true};
			bool[] row3 = {false, false, true, false, false};
			bool[] row4 = {false, false, true, false, false};
			viableLocations[0] = row0;
			viableLocations[1] = row1;
			viableLocations[2] = row2;
			viableLocations[3] = row3;
			viableLocations[4] = row4;
		} else if(troopType == "bishop")
		{
			// Moves like a bishop from chess
			bool[] row0 = {true, false, false, false, true};
			bool[] row1 = {false, true, false, true, false};
			bool[] row2 = {false, false, startPos, false, false};
			bool[] row3 = {false, true, false, true, false};
			bool[] row4 = {true, false, false, false, true};
			viableLocations[0] = row0;
			viableLocations[1] = row1;
			viableLocations[2] = row2;
			viableLocations[3] = row3;
			viableLocations[4] = row4;
		}
	}

	public void Drag(Vector2 mousePos)
	{
		if(dragStartLocation == null)
		{
			dragStartLocation = GlobalPosition;
		}
		WorldGridManager.MoveOwner(this, mousePos);
	}

	// Should be able to move only X amount of tiles. How to do?
	public void Release()
	{
		if(CheckForCell(GetGlobalMousePosition(), (Vector2)dragStartLocation) && !hasMoved)
		{
			WorldGridManager.MoveOwner(this,WorldGridManager.CalculateSnapPos(GetGlobalMousePosition()));
			hasMoved = true;
		} else 
		{
			if(hasMoved)
			{
				Hud.instance.ShowMessage("You have already moved this troop on this turn!");
			} else
			{
				Hud.instance.ShowMessage("Unable to move this troop to that position.");
			}
			WorldGridManager.MoveOwner(this, (Vector2)dragStartLocation);
		}
		
		dragStartLocation = null;
	}

	private bool CheckForCell(Vector2 releasePos, Vector2 startPos)
	{
		// laske, sijaitseeko release position suhteessa lähtöpositioniin arvolla true vai false 2D arraysta
		var offset = WorldGridManager.GetNearestCell(releasePos) - WorldGridManager.GetNearestCell(startPos);

		// Debug
		GD.Print("Offset: "+offset);
		GD.Print("Release cell " + WorldGridManager.GetNearestCell(releasePos));
		//GD.Print("Start cell "+WorldGridManager.GetNearestCell((Vector2)dragStartLocation));

		// Check for whether or not player is trying to move the troop too far away from original position
		if(2 + (int)offset.Y >= viableLocations.Length || 2 + (int)offset.X >= viableLocations.Length) return false;

		if(viableLocations[2 + (int)offset.Y][2 + (int)offset.X])
		{
			return true;
		} else
		{
			return false;
		}
	}
}
