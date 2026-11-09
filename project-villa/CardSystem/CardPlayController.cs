using Godot;
using System;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class CardPlayController : Node
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

        if (!card.CanPlay(this))
        {
            Hud?.ShowCardMessage("You can't play that card right now.");
            return false;
        }

        if (card.Cost > 0 && (Hud == null || !Hud.TrySpendResources(card.Cost)))
        {
            Hud?.ShowCardMessage("Not enough resources.");
            return false;
        }

        try
        {
            card.Play(this);
        }
        catch (Exception error)
        {
            if (card.Cost > 0)
            {
                Hud?.AddResources(card.Cost);
            }

            GD.PushError("Card failed: " + error.Message);
            Hud?.ShowCardMessage("The card could not be played.");
            return false;
        }

        Hud?.ShowCardMessage("Played " + card.DisplayName + ".");
        return true;
    }
}
