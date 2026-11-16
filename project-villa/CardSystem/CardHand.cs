using Godot;
using System;
using System.Collections.Generic;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class CardHand : Control
{
	[Export] public PackedScene CardViewScene;
	[Export] public CardDeck StartingDeck;
	[Export] public int StartingHandSize = 4;
	[Export] public NodePath ControllerPath = "../../../../CardSystem/CardPlayController";

	[Export] public CardPlayController Controller { get; private set; }

	private readonly List<CardView> _hand = new();
	private readonly List<Card> _drawPile = new();
	private readonly Random _random = new();

	public int HandCount => _hand.Count;
	public int DeckCount => _drawPile.Count;
	public int CardsAvailable => _drawPile.Count;

	public override void _Ready()
	{
		if(Controller == null)
		{
			Controller = GetNode<CardPlayController>(ControllerPath);
		}
		

		if (StartingDeck != null)
		{
			LoadDeck(StartingDeck, StartingHandSize);
		}
	}

	public override void _Process(double delta)
	{
		LayoutCards();
	}

	public void LoadDeck(CardDeck deck, int handSize)
	{
		ClearHand();
		StartingHandSize = handSize;

		foreach (Card card in deck.Cards)
		{
			if (card != null)
			{
				_drawPile.Add(card);
			}
		}

		Shuffle(_drawPile);
		DrawCards(handSize);
	}

	public void AddCard(Card card)
	{
		if (card == null)
		{
			return;
		}

		_drawPile.Add(card);
		Shuffle(_drawPile);
		DrawCards(StartingHandSize - _hand.Count);
	}

	public int DrawCards(int count)
	{
		int drawn = 0;

		for (int i = 0; i < count; i++)
		{
			if (_drawPile.Count == 0)
			{
				break;
			}

			Card card = _drawPile[0];
			_drawPile.RemoveAt(0);
			AddCardView(card);
			drawn++;
		}

		UpdateHud();
		return drawn;
	}

	public void PlayCard(CardView view)
	{
		if (view == null || view.Card == null)
		{
			return;
		}

		if (Controller == null || !Controller.TryPlay(view.Card))
		{
			view.ZIndex = 0;
			return;
		} 

		_hand.Remove(view);
		view.QueueFree();

		DrawCards(StartingHandSize - _hand.Count);
	}

	private void AddCardView(Card card)
	{
		if (CardViewScene == null)
		{
			GD.PushError("CardHand has no CardViewScene.");
			return;
		}

		CardView view = CardViewScene.Instantiate<CardView>();
		AddChild(view);
		view.Hand = this;
		view.SetCard(card);
		_hand.Add(view);
		LayoutCards();
	}

	private void LayoutCards()
	{
		int count = _hand.Count;
		if (count == 0)
		{
			return;
		}

		float width = Size.X > 1f ? Size.X : 600f;
		float height = Size.Y > 1f ? Size.Y : 200f;
		float cardWidth = _hand[0].Size.X > 1f ? _hand[0].Size.X : 132f;
		float cardHeight = _hand[0].Size.Y > 1f ? _hand[0].Size.Y : 184f;
		float spacing = count > 1
			? Mathf.Min(cardWidth * 0.75f, (width - cardWidth - 16f) / (count - 1))
			: 0f;
		float total = cardWidth + spacing * (count - 1);
		float startX = (width - total) * 0.5f;
		float y = height - cardHeight - 8f;

		for (int i = 0; i < count; i++)
		{
			CardView view = _hand[i];

			if (!view.Dragging)
			{
				view.Position = new Vector2(startX + spacing * i, y);
			}
		}
	}

	private void Shuffle(List<Card> cards)
	{
		for (int i = cards.Count - 1; i > 0; i--)
		{
			int j = _random.Next(i + 1);
			(cards[i], cards[j]) = (cards[j], cards[i]);
		}
	}

	private void ClearHand()
	{
		foreach (CardView view in _hand)
		{
			view.QueueFree();
		}

		_hand.Clear();
		_drawPile.Clear();
	}

	private void UpdateHud()
	{
		if (Controller != null && Controller.Hud != null)
		{
			Controller.Hud.SetDeckCount(DeckCount, _hand.Count);
		}
	}
}
