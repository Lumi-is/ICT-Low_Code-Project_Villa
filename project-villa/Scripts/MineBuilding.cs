using Godot;
using Grid;
using System;

[GlobalClass]

public partial class MineBuilding : Node2D, IBuilding
{
	[Export] int stoneAmount = 50;
	
	public void OnBuild()
	{
		WorldGridManager.buildingList.Add(this);
	}

	public void OnTurnStart()
	{
		ResourceManager.Add(ResourceManager.ResourceType.Stone, stoneAmount);
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