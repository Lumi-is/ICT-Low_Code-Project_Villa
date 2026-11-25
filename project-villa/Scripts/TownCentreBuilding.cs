using Godot;
using Grid;
using System;

[GlobalClass]


// Draw cards at the start of every turn based on TownCentreLevel.
// Level is defined by the number of housing.
// For each to check WordGridManager, BuildingList for each building that has IBuilding, then check whick IBuilding is housing.
// Where does the game draw cards? use that to draw cards.

public partial class TownCentreBuilding : Node2D, IBuilding
{
	[Export]
	public int level2Requirement = 5;
	[Export]
	public int level3Requirement = 15;
	public static int amountOfHousing;
	public override void _Process(double delta)
	{
		amountOfHousing = 0;
		
		foreach (IBuilding building in WorldGridManager.buildingList)
		{
			if (building is HousingBuilding)
			{
				amountOfHousing += 1;
			}

			if (amountOfHousing >= level2Requirement)
			{
				
			}

			if (amountOfHousing >= level3Requirement)
			{
				
			}
		}
	}
	
	public void OnBuild()
	{
		
	}

	public void OnTurnStart()
	{
		
	}
}