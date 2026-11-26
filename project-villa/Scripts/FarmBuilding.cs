using Godot;
using System;

[GlobalClass]

public partial class FarmBuilding : Node2D, IBuilding
{
	[Export] int foodAmount = 50;
	
	public void OnBuild()
	{
		
	}

	public void OnTurnStart()
	{
		ResourceManager.Add(ResourceManager.ResourceType.Food, foodAmount);
		Hud.instance?.UpdateResourcesUI();
	}
}