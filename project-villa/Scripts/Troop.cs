using System;
using Godot;
using Global;

public partial class Troop : Node2D, IClickable, IDraggable
{
	[Export] public bool isPlayerControlled = true;
	[Export] public string troopType;
	[Export] public Sprite2D graphics;

	public bool[][] viableLocations = new bool[5][];
	private Vector2? dragStartLocation = null;
	const bool startPos = false;

	public bool hasMoved;


	public void OnSpawn()
	{
		WorldManager.troopList.Add(this);
		WorldManager.troopGrid[WorldManager.GetNearestCell(GlobalPosition)[0]+256,WorldManager.GetNearestCell(GlobalPosition)[1]+256] = this;

		// Debug
		var nearestCell = WorldManager.GetNearestCell(GlobalPosition);
		GD.Print(WorldManager.troopGrid[nearestCell[0] + 256, nearestCell[1] + 256].Name + " was spawned to position "+nearestCell);
	}

	private void OnDestroy()
	{
		WorldManager.troopList.Remove(this);
	}

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
		OnSpawn();

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
		if(!isPlayerControlled) return;

		if(dragStartLocation == null)
		{
			dragStartLocation = GlobalPosition;
		}
		GlobalPosition = mousePos;
	}

	public void Destruct()
	{
		OnDestroy();
		GD.Print(Name + " is about to get destroyed.");
		QueueFree();
		GD.Print("Poof!");
	}

	// Should be able to move only X amount of tiles. How to do?
	public void Release()
	{
		if(!isPlayerControlled) return;

		if(CheckForCell(GetGlobalMousePosition(), (Vector2)dragStartLocation) && !hasMoved)
		{
			// Make cards for creating troops, make troops add themselves to grid 
			var startPos = (Vector2)dragStartLocation;
			var nearestCellToStart = WorldManager.GetNearestCell(startPos);
			var releasePos = GetGlobalMousePosition();
			var nearestCellToRelease = WorldManager.GetNearestCell(releasePos);

			WorldManager.troopGrid[nearestCellToStart.X+256, nearestCellToStart.Y+256] = null;
			WorldManager.troopGrid[nearestCellToRelease.X+256,nearestCellToRelease.Y+256] = this;
			Tile currentTile = WorldManager.GetTileAtWorldPosition(releasePos);
			Reparent(currentTile);
			GlobalPosition = currentTile.GlobalPosition; 

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
			GlobalPosition = (Vector2)dragStartLocation;
		}
		
		dragStartLocation = null;
	}

	private bool CheckForCell(Vector2 releasePos, Vector2 startPos)
	{
		var nearestCellToRelease = WorldManager.GetNearestCell(releasePos);
		var nearestCellToStart = WorldManager.GetNearestCell(startPos);
		int[] cellOffset = [nearestCellToRelease[0]-nearestCellToStart[0],nearestCellToRelease[1]-nearestCellToStart[1]];

		// Check for whether or not player is trying to move the troop too far away from original position
		if(2 + cellOffset[0] >= viableLocations.Length || 2 + cellOffset[1] >= viableLocations.Length || 2 - cellOffset[0] < 0 || 2 - cellOffset[1] < 0) return false;

		Type cellContents = WorldManager.GetCellContentsType(releasePos);

		if(viableLocations[2 + cellOffset[1]][2 + cellOffset[0]] && cellContents == null)
		{
			
			return true;
		} else if(WorldManager.GetCellContentsType(releasePos) == typeof(Troop))
		{
			Troop troop = WorldManager.troopGrid[nearestCellToRelease[0]+256,nearestCellToRelease[1]+256];
			if(!troop.isPlayerControlled)
			{
				Hud.instance.ShowMessage("Enemy troop was destroyed.");
				troop.Destruct(); 
				return true;
			} else
			{
				return false;
			}
			
		} else
		{
			return false;
		}
	}
}

