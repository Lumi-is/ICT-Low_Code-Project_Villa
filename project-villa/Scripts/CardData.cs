using Godot;

[GlobalClass]
public partial class CardData : Resource
{
	public int woodCost;
	public int stoneCost;
	public int magicCost;

	[Export] public string CardName { get; set; } = "New Card";
	[Export] public Texture2D Icon { get; set; }
	[Export] public string Description { get; set; } = "";
}
