using Godot;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class Card : Resource
{
	[Export] public string Id = "";
	[Export] public string DisplayName = "New Card";
	[Export(PropertyHint.MultilineText)] public string Description = "";
	[Export] public string Category = "Action";
	[Export] public Color AccentColor = new Color("#e0b85a");
	[Export] public int MagicCost = 0;
	[Export] public int ShopPrice = 10;

	public virtual bool TryPlay(CardPlayController game, Vector2 playPos)
	{
		return true;
	}
}
