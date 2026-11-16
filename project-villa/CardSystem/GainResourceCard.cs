
using Godot;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class GainResourceCard : Card
{
	[Export] public int Amount = 1;

	public override bool CanPlay(CardPlayController game)
	{
		return Amount > 0 && game.Hud != null;
	}

	public override void Play(CardPlayController game, Vector2 vector2, Node node)
	{
		game.Hud.AddResources(Amount);
	}
}
