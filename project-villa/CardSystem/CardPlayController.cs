using Godot;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class CardPlayController : Node2D
{
	[Export] public NodePath HudPath = "../..";
	[Export] public NodePath HandPath = "../../BottomPanel/HBoxContainer/CardHandSection/CardHand";

	public Hud Hud { get; private set; }
	public CardHand Hand { get; private set; }

	public override void _Ready()
	{
		Hud = GetNodeOrNull<Hud>(HudPath);
		Hand = GetNodeOrNull<CardHand>(HandPath);
	}

	public bool TryPlay(Card card, Vector2 playPos)
	{
		if (card == null)
		{
			return false;
		}

		if (!ResourceManager.CanPlayCard(card))
		{
			Hud?.ShowMessage("Not enough resources to play " + card.DisplayName + ".");
			return false;
		}

		int messageVersion = Hud?.MessageVersion ?? 0;

		if (!card.TryPlay(this, playPos))
		{
			if (Hud != null && Hud.MessageVersion == messageVersion)
			{
				Hud.ShowMessage("The card could not be played.");
			}

			return false;
		}

		ResourceManager.SpendCardCost(card);
		Hud?.UpdateResourcesUI();

		if (Hud != null && Hud.MessageVersion == messageVersion)
		{
			Hud.ShowMessage("Played " + card.DisplayName + ".");
		}

		return true;
	}
}
