using Godot;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class DrawCardsCard : Card
{
	[Export] public int Count = 1;


	public override bool TryPlay(CardPlayController game, Vector2 vector2)
	{
		if(Count > 0 && game.Hand != null && game.Hand.CardsAvailable > 0)
		{
			game.Hand.DrawCards(Count);
			return true;
		} else
		{
			return false;
		}
		
	}
}
