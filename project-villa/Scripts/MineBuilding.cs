using Godot;
using System;

[GlobalClass]

public partial class MineBuilding : Node2D, IBuilding
{
	[Export] int stoneAmount = 50;
	
	public void OnBuild()
	{
		
	}

	public void OnTurnStart()
	{
		ResourceManager.Add(ResourceManager.ResourceType.Stone, stoneAmount);
	}
}