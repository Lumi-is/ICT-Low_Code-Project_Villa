using Godot;
using Grid;
using System;

public partial class Building : Node2D
{
	public virtual void OnBuild()
	{
		

		var nearestCell = WorldGridManager.GetNearestCell(GlobalPosition);

		// Debug
		GD.Print(WorldGridManager.buildingGrid[nearestCell[0] + 256, nearestCell[1] + 256].Name + " was built to position "+nearestCell);
	}

	public virtual void OnDestroy()
	{
		
	}

	public virtual void OnTurnStart()
	{
		
	}
}
