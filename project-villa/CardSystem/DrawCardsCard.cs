using Godot;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class DrawCardsCard : Card
{
	[Export] public int Count = 1;

	public override bool CanPlay(CardPlayController game)
	{
		return Count > 0 && game.Hand != null && game.Hand.CardsAvailable > 0;
	}

	public override void Play(CardPlayController game)
	{
		game.Hand.DrawCards(Count);
	}
}
