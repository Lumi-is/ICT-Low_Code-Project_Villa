using Godot;
using System;

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

	public bool TryPlay(Card card)
	{
		if (card == null)
		{
			return false;
		}

		/*if (!card.CanPlay(this))
		{
			Hud?.ShowCardMessage("You can't play that card right now.");
			return false;
		}*/

		if(ResourceManager.ResourceCalculate(card))
		{
			card.Play(this, GetGlobalMousePosition(), GetNode("/root")); 
			Hud?.ShowCardMessage(card.DisplayName + " was played succesfully.");
			return true;
		} else
		{
			GD.PushError("Failed to play card.");
			Hud?.ShowCardMessage("The card could not be played.");
			return false;
		}
	}
}
