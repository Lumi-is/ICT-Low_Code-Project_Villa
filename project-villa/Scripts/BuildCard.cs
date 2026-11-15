using Godot;
using Grid;
using ProjectVilla.Cards;
using System;

[GlobalClass]
public partial class BuildCard : Card
{
    [Export] string path = "res://Scenes/DefaultBuilding.tscn";
    public override void Play(CardPlayController game, Vector2 playPos, Node node)
    {
        base.Play(game, playPos, node);

        PackedScene scene;
        scene = ResourceLoader.Load<PackedScene>(path);


        Node2D building = scene.Instantiate() as Node2D;
        node.AddChild(building);
        
        WorldGridManager.SnapToGrid(building, WorldGridManager.CalculateSnapPos(playPos));
    }
}
