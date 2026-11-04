using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class ShopMenu : CanvasLayer
{
	[ExportGroup("UI References")]
	[Export] public Container CardContainer;
	[Export] public Button RefreshButton;
	[Export] public Button CloseButton;

	[ExportGroup("Shop Configuration")]
	[Export] public PackedScene CardDisplayPrefab;
	[Export] public int RefreshCost = 5;
	[Export] public Godot.Collections.Array<CardData> AvailableCardPool = new();

	[Signal] public delegate void CardPurchasedEventHandler(CardData card);
	[Signal] public delegate void RefreshRequestedEventHandler(int cost);

	private List<CardData> _currentShopCards = new();
	private RandomNumberGenerator _rng = new();

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		_rng.Randomize();

		CardContainer ??= GetNode<Container>("UI/Background/VBoxContainer/CardContainer");
		RefreshButton ??= GetNode<Button>("UI/Background/VBoxContainer/ButtonContainer/RefreshButton");
		CloseButton ??= GetNode<Button>("UI/Background/VBoxContainer/ButtonContainer/CloseButton");

		RefreshButton.Pressed += OnRefreshButtonPressed;
		CloseButton.Pressed += OnCloseButtonPressed;

		Hide();
	}

	public void OpenShop()
	{
		Show();
		GenerateNewShopSelection();
	}

	public void CloseShop()
	{
		Hide();
	}

	public void GenerateNewShopSelection()
	{
		if (CardContainer == null || CardDisplayPrefab == null || AvailableCardPool.Count == 0)
		{
			GD.Print("Shop is not configured yet (CardDisplayPrefab or AvailableCardPool is empty).");
			return;
		}

		_currentShopCards.Clear();

		foreach (Node child in CardContainer.GetChildren())
		{
			child.QueueFree();
		}

		for (int i = 0; i < 3; i++)
		{
			int randomIndex = _rng.RandiRange(0, AvailableCardPool.Count - 1);
			CardData selectedCard = AvailableCardPool[randomIndex];
			_currentShopCards.Add(selectedCard);

			var cardUI = CardDisplayPrefab.Instantiate();
			CardContainer.AddChild(cardUI);

			if (cardUI.HasMethod("SetupCard"))
			{
				cardUI.Call("SetupCard", selectedCard);
			}
		}

		GD.Print("Shop refreshed with 3 new cards!");
	}

	private void OnRefreshButtonPressed()
	{
		EmitSignal(SignalName.RefreshRequested, RefreshCost);
		GenerateNewShopSelection();
	}

	private void OnCloseButtonPressed()
	{
		CloseShop();
	}
}
