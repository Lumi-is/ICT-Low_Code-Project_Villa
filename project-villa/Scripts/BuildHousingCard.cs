using Godot;
using Grid;
using ProjectVilla.Cards;

[GlobalClass]
public partial class BuildHousingCard : Card
{
    [Export] string path = "res://Scenes/DefaultBuilding.tscn";
    public override void Play(CardPlayController game, Vector2 playPos, Node node)
    {
        WorldGridManager.InstantiateObject(playPos,node, path);
    }
}
