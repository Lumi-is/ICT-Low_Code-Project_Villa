using Godot;
using Grid;
using System;

[GlobalClass]

public partial class FarmBuilding : Building
{
	[Export] int foodAmount = 50;

	public override void OnTurnStart()
	{
		ResourceManager.Add(ResourceManager.ResourceType.Food, foodAmount);
		Hud.instance?.UpdateResourcesUI();
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