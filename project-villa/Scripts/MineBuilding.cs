using Godot;
using Grid;
using System;

[GlobalClass]

public partial class MineBuilding : Building
{
	[Export] int stoneAmount = 50;
	
	

	public override void OnTurnStart()
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