using Godot;
using Grid;
using ProjectVilla.Cards;

[GlobalClass]
public partial class BuildCard : Card
{
    [Export] string path = "res://Scenes/DefaultBuilding.tscn";
    public override bool TryPlay(CardPlayController game, Vector2 playPos)
    {
        return WorldGridManager.InstantiateObject(playPos, path);
    }
}
