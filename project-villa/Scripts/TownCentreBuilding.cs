using Godot;
using Grid;
using System;

[GlobalClass]


// Draw cards at the start of every turn based on TownCentreLevel.
// Level is defined by the number of housing.
// For each to check WordGridManager, BuildingList for each building that has IBuilding, then check whick IBuilding is housing.


public partial class TownCentreBuilding : Node2D, IBuilding
{
	public override void _Process(double delta)
	{
		foreach (IBuilding building in WorldGridManager.buildingList)
		{
			
		}
	}
	
	public void OnBuild()
	{
		
	}

	public void OnTurnStart()
	{
		
	}
}