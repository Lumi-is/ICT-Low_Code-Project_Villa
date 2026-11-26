using Godot;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class DrawCardsCard : Card
{
	[Export] public int Count = 2;

	public override bool TryPlay(CardPlayController game, Vector2 playPos)
	{
		if (Count <= 0 || game == null)
		{
			return false;
		}

		return game.Hud?.GiveCards(Count) > 0;
	}
}
