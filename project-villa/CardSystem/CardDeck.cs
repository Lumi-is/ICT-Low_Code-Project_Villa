using Godot;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class CardDeck : Resource
{
	[Export] public Godot.Collections.Array<Card> Cards = new();
}
