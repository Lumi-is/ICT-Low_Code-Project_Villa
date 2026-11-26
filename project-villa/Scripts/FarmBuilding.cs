using Godot;
using Grid;
using System;

[GlobalClass]

public partial class FarmBuilding : Node2D, IBuilding
{
	[Export] int foodAmount = 50;
	
	public void OnBuild()
	{
		WorldGridManager.buildingList.Add(this);
	}

	public void OnTurnStart()
	{
		ResourceManager.Add(ResourceManager.ResourceType.Food, foodAmount);
	}

	public void Highlight()
    {
        
    }

    public void Reset()
    {
        
    }

    public void OnClickEvent()
    {
        
    }

    public Sprite2D GetGraphics()
    {
        return null;
    }
}