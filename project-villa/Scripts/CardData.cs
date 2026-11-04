using Godot;

[GlobalClass]
public partial class CardData : Resource
{
    [Export] public string CardName { get; set; } = "New Card";
    [Export] public int Cost { get; set; } = 10;
    [Export] public Texture2D Icon { get; set; }
    [Export] public string Description { get; set; } = "";
}