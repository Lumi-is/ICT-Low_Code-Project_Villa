using Godot;
using Grid;

[GlobalClass]
public partial class HousingBuilding : Node2D, IBuilding
{
    public void OnBuild()
    {
        WorldGridManager.buildingList.Add(this);
    }

    public void OnTurnStart()
    {
        
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
