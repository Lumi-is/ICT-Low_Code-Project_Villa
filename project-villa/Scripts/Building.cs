using Godot;
using Global;
using System;

public partial class Building : Node2D
{
	public virtual void OnBuild()
	{
		

		var nearestCell = WorldManager.GetNearestCell(GlobalPosition);

		// Debug
		GD.Print(WorldManager.buildingGrid[nearestCell[0] + 256, nearestCell[1] + 256].Name + " was built to position "+nearestCell);
	}

	public void Destruct()
	{
		OnDestroy();
		QueueFree();
	}

	public virtual void OnDestroy()
	{
		WorldManager.buildingList.Remove(this);
	}

	public virtual void OnTurnStart()
	{
		
	}
}
