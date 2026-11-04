using Godot;
using System.Collections.Generic;

public partial class Hud : CanvasLayer
{
    // Node references matching the HUD scene tree.
    [ExportGroup("UI References")]
    [Export] public Button PauseButton;
    [Export] public Button ShopButton;
    [Export] public Label DeckLabel;
    [Export] public Label ResourcesLabel;
    [Export] public Container CardContainer;

    [ExportGroup("Menus")]
    [Export] public PauseMenu PauseMenu;
    [Export] public ShopMenu ShopMenu;

    // Prefab for instantiating cards into slots (assign in Inspector if spawning dynamically).
    [ExportGroup("Card Settings")]
    [Export] public PackedScene CardPrefab;

    // Example Game State Data
    private int _resourceCount = 100;
    private readonly List<string> _deck = new() { "Card 1", "Card 2", "Card 3", "Card 4" };
    private readonly List<string> _hand = new();

    public override void _Ready()
    {
        // Keep the HUD responsive even when the game tree is paused.
        ProcessMode = ProcessModeEnum.Always;

        // 1. Automatic node fetching if not explicitly assigned in Inspector.
        PauseButton ??= GetNode<Button>("BottomPanel/HBoxContainer/InteractiveButtons/VBoxContainer/Pause");
        ShopButton ??= GetNode<Button>("BottomPanel/HBoxContainer/InteractiveButtons/VBoxContainer/Shop");
        DeckLabel ??= GetNode<Label>("BottomPanel/HBoxContainer/Stats/HBoxContainer/deck");
        ResourcesLabel ??= GetNode<Label>("BottomPanel/HBoxContainer/Stats/HBoxContainer/resources");
        CardContainer ??= GetNode<Container>("BottomPanel/HBoxContainer/CardHandSection/CardContainer");

        // PauseMenu and ShopMenu are instanced as children of the HUD scene.
        PauseMenu ??= GetNodeOrNull<PauseMenu>("PauseMenu");
        ShopMenu ??= GetNodeOrNull<ShopMenu>("ShopMenu");

        // 2. Connect Button Signals.
        PauseButton.Pressed += OnPauseButtonPressed;
        ShopButton.Pressed += OnShopButtonPressed;

        // 3. Initial UI update.
        UpdateResourcesUI();
        UpdateDeckUI();
    }

    // --- Button Callbacks ---

    private void OnPauseButtonPressed()
    {
        GD.Print("Pause button clicked");
        PauseMenu?.TogglePause();
    }

    private void OnShopButtonPressed()
    {
        ShopMenu?.OpenShop();
    }

    // --- Data & Resource Display ---

    public void UpdateResourcesUI()
    {
        if (ResourcesLabel != null)
        {
            ResourcesLabel.Text = $"Resources: {_resourceCount}";
        }
    }

    public void UpdateDeckUI()
    {
        if (DeckLabel != null)
        {
            DeckLabel.Text = $"Deck: {_deck.Count}";
        }
    }

    public void AddResources(int amount)
    {
        _resourceCount += amount;
        UpdateResourcesUI();
    }

    // --- Card & Hand Management ---

    /// <summary>
    /// Draws a card from the deck and adds it visually to CardContainer.
    /// </summary>
    public void DrawCard()
    {
        if (_deck.Count == 0)
        {
            GD.Print("Deck is empty!");
            return;
        }

        // Draw top card from deck.
        string drawnCardData = _deck[0];
        _deck.RemoveAt(0);
        _hand.Add(drawnCardData);

        // Instantiate card UI into CardContainer.
        if (CardPrefab != null && CardContainer != null)
        {
            Control cardInstance = CardPrefab.Instantiate<Control>();
            CardContainer.AddChild(cardInstance);
        }

        UpdateDeckUI();
    }
}
