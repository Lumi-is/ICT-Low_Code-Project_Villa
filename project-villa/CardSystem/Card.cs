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
	[Export] public int Cost = 0;

	public virtual bool CanPlay(CardPlayController game)
	{
		return true;
	}

	public virtual void Play(CardPlayController game)
	{
	}
}
