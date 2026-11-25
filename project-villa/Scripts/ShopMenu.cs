using Godot;
using ProjectVilla.Cards;

[GlobalClass]
public partial class ShopMenu : CanvasLayer
{
	[ExportGroup("UI References")]
	[Export] public Container CardContainer;
	[Export] public Label ResourcesLabel;
	[Export] public Button RefreshButton;
	[Export] public Button CloseButton;

	[ExportGroup("Shop Configuration")]
	[Export] public PackedScene CardDisplayPrefab;
	[Export] public int ShopSize = 3;
	[Export] public int RefreshCost = 5;
	[Export] public Godot.Collections.Array<Card> AvailableCardPool = new();

	private readonly RandomNumberGenerator _rng = new();
	private bool _generated;

 [Export] public string HandPath = "../HBoxContainer/CardHandSection/CardHand";

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		_rng.Randomize();

		CardContainer ??= GetNode<Container>("UI/Background/VBoxContainer/CardContainer");
		ResourcesLabel ??= GetNode<Label>("UI/Background/VBoxContainer/ResourcesLabel");
		RefreshButton ??= GetNode<Button>("UI/Background/VBoxContainer/ButtonContainer/RefreshButton");
		CloseButton ??= GetNode<Button>("UI/Background/VBoxContainer/ButtonContainer/CloseButton");

		RefreshButton.Pressed += OnRefreshButtonPressed;
		CloseButton.Pressed += OnCloseButtonPressed;

		RefreshButton.Text = RefreshCost > 0 ? $"Refresh ({RefreshCost})" : "Refresh";

		Hide();
	}

	public void OpenShop()
	{
		Show();
		UpdateResourcesLabel();

		if (!_generated)
		{
			GenerateNewShopSelection();
		}
	}

	public void CloseShop()
	{
		Hide();
	}

	private bool GenerateNewShopSelection()
	{
		if (CardContainer == null || CardDisplayPrefab == null || AvailableCardPool.Count == 0)
		{
			GD.PushWarning("Shop has no card display prefab or no available cards.");
			return false;
		}

		foreach (Node child in CardContainer.GetChildren())
		{
			child.QueueFree();
		}

		for (int i = 0; i < ShopSize; i++)
		{
			Card card = AvailableCardPool[_rng.RandiRange(0, AvailableCardPool.Count - 1)];

			if (card == null)
			{
				continue;
			}

			ShopCardView view = CardDisplayPrefab.Instantiate<ShopCardView>();
			CardContainer.AddChild(view);
			view.SetCard(card);
			view.BuyRequested += OnBuyRequested;
		}

		_generated = true;
		UpdateResourcesLabel();
		return true;
	}

	private void OnBuyRequested(ShopCardView view)
	{
		Hud hud = GetHud();

		if (hud == null || view == null || view.Card == null)
		{
			return;
		}

		CardHand hand = GetNodeOrNull<CardHand>(HandPath);

		if (hand == null)
		{
			hud.ShowMessage("There is no deck to add the card to.");
			return;
		}

		Card card = view.Card;

		if (!hud.TrySpendResources(card.ShopPrice))
		{
			hud.ShowMessage("Not enough resources.");
			return;
		}

		hand.AddCard(card);
		hud.ShowMessage("Bought " + card.DisplayName + ".");
		UpdateResourcesLabel();

		CardContainer.RemoveChild(view);
		view.QueueFree();
	}

	private void OnRefreshButtonPressed()
	{
		Hud hud = GetHud();

		if (hud == null)
		{
			return;
		}

		if (!hud.TrySpendResources(RefreshCost))
		{
			hud.ShowMessage("Not enough resources to refresh.");
			return;
		}

		if (!GenerateNewShopSelection())
		{
			hud.AddResources(RefreshCost);
			return;
		}

		hud.ShowMessage("Shop refreshed.");
		UpdateResourcesLabel();
	}

	private void OnCloseButtonPressed()
	{
		CloseShop();
	}

	private Hud GetHud()
	{
		return GetParent() as Hud;
	}

	private void UpdateResourcesLabel()
	{
		Hud hud = GetHud();

		if (ResourcesLabel != null && hud != null)
		{
			ResourcesLabel.Text = "Magic: " + hud.Resources;
		}
	}
}
