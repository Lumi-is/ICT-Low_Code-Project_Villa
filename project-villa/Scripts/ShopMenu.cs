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
    [Export] public PackedScene CardDisplayPrefab; // Scene for individual card display
    [Export] public int RefreshCost = 5;
    [Export] public Godot.Collections.Array<CardData> AvailableCardPool = new();

    // Event signals to notify HUD/Game Controller
    [Signal] public delegate void CardPurchasedEventHandler(CardData card);
    [Signal] public delegate void RefreshRequestedEventHandler(int cost);

    private List<CardData> _currentShopCards = new();
    private RandomNumberGenerator _rng = new();

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        _rng.Randomize();

        // Node auto-fetching.
        CardContainer ??= GetNode<Container>("UI/Background/VBoxContainer/CardContainer");
        RefreshButton ??= GetNode<Button>("UI/Background/VBoxContainer/ButtonContainer/RefreshButton");
        CloseButton ??= GetNode<Button>("UI/Background/VBoxContainer/ButtonContainer/CloseButton");

        // Connect button signals.
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

    /// <summary>
    /// Randomly picks 3 cards from the pool to display in the shop.
    /// </summary>
    public void GenerateNewShopSelection()
    {
        // Keep the placeholder slots visible until a card prefab and pool are set up.
        if (CardContainer == null || CardDisplayPrefab == null || AvailableCardPool.Count == 0)
        {
            GD.Print("Shop is not configured yet (CardDisplayPrefab or AvailableCardPool is empty).");
            return;
        }

        _currentShopCards.Clear();

        // Clear existing visual card nodes.
        foreach (Node child in CardContainer.GetChildren())
        {
            child.QueueFree();
        }

        // Select 3 cards randomly.
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = _rng.RandiRange(0, AvailableCardPool.Count - 1);
            CardData selectedCard = AvailableCardPool[randomIndex];
            _currentShopCards.Add(selectedCard);

            // Instantiate visual display slot.
            var cardUI = CardDisplayPrefab.Instantiate();
            CardContainer.AddChild(cardUI);

            // If card UI has a script, pass data into it.
            if (cardUI.HasMethod("SetupCard"))
            {
                cardUI.Call("SetupCard", selectedCard);
            }
        }

        GD.Print("Shop refreshed with 3 new cards!");
    }

    // --- Button Callbacks ---

    private void OnRefreshButtonPressed()
    {
        // Emit signal so HUD can check if player has enough resources/gold.
        EmitSignal(SignalName.RefreshRequested, RefreshCost);
        GenerateNewShopSelection();
    }

    private void OnCloseButtonPressed()
    {
        CloseShop();
    }
}
