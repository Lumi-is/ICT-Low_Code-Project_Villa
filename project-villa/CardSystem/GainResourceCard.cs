
using Godot;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class GainResourceCard : Card
{
	[Export] public int Amount = 1;

	public override bool TryPlay(CardPlayController game, Vector2 vector2)
	{
		if(Amount > 0 && game.Hud != null)
		{
			game.Hud.AddResources(Amount);
			return true;
		}
		else
		{
			return false;
		}
		
	}
}
