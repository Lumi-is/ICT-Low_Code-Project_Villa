using Godot;
using System;
using System.Collections.Generic;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class CardHand : Control
{
	[Export] public PackedScene CardViewScene;
	[Export] public CardDeck StartingDeck;
	[Export] public int HandSize = 4;
	[Export] public NodePath ControllerPath = "../../../../CardSystem/CardPlayController";

	[Export] public CardPlayController Controller { get; private set; }

	private readonly List<CardView> _hand = new();
	private readonly List<Card> _drawPile = new();
	private readonly Random _random = new();

	public int HandCount => _hand.Count;
	public int DeckCount => _drawPile.Count;
	public int CardsAvailable => _drawPile.Count;
	public int HandCapacity => Mathf.Max(0, HandSize);
	public bool IsLoaded { get; private set; }

	public override void _Ready()
	{
		if (Controller == null)
		{
			Controller = GetNode<CardPlayController>(ControllerPath);
		}

		EnsureLoaded();
	}

	public void EnsureLoaded()
	{
		if (IsLoaded)
		{
			return;
		}

		IsLoaded = true;

		if (StartingDeck != null)
		{
			LoadDeck(StartingDeck);
		}
	}

	public override void _Process(double delta)
	{
		LayoutCards();
	}

	public void LoadDeck(CardDeck deck)
	{
		ClearHand();

		foreach (Card card in deck.Cards)
		{
			if (card != null)
			{
				_drawPile.Add(card);
			}
		}

		Shuffle(_drawPile);
		DrawCards(HandCapacity);
	}

	public void AddCard(Card card)
	{
		if (card == null)
		{
			return;
		}

		EnsureLoaded();

		if (!AddCardToHandOrDeck(card))
		{
			Shuffle(_drawPile);
		}

		UpdateHud();
	}

	public int GiveCards(int amount, Godot.Collections.Array<Card> cardPool)
	{
		EnsureLoaded();

		if (amount <= 0 || cardPool == null)
		{
			return 0;
		}

		List<Card> choices = new();

		foreach (Card card in cardPool)
		{
			if (card != null)
			{
				choices.Add(card);
			}
		}

		if (choices.Count == 0)
		{
			return 0;
		}

		int addedToHand = 0;

		for (int i = 0; i < amount; i++)
		{
			Card card = choices[_random.Next(choices.Count)];

			if (AddCardToHandOrDeck(card))
			{
				addedToHand++;
			}
		}

		Shuffle(_drawPile);
		UpdateHud();
		return addedToHand;
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

	public void PlayCard(CardView view, Vector2 playPos)
	{
		if (view == null || view.Card == null)
		{
			return;
		}

		if (Controller == null || !Controller.TryPlay(view.Card, playPos))
		{
			view.ZIndex = 0;
			return;
		}

		_hand.Remove(view);
		view.QueueFree();

		DrawCards(HandCapacity - _hand.Count);
	}

	private bool AddCardToHandOrDeck(Card card)
	{
		if (_hand.Count < HandCapacity && CardViewScene != null)
		{
			AddCardView(card);
			return true;
		}

		_drawPile.Add(card);
		return false;
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
