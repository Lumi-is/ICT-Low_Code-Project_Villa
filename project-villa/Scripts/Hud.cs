using System.Collections.Generic;
using Godot;
using ProjectVilla.Cards;

[GlobalClass]
public partial class Hud : CanvasLayer
{
	public static Hud instance;
	[ExportGroup("UI References")]
	[Export] public Button PauseButton;
	[Export] public Button ShopButton;
	[Export] public Button PassTurnButton;
	[Export] public Label TimeOfDay;
	[Export] public RichTextLabel DeckLabel;
	[Export] public RichTextLabel ResourcesLabel;
	[Export] public Label CardMessageLabel;

	[ExportGroup("Menus")]
	[Export] public PauseMenu PauseMenu;
	[Export] public ShopMenu ShopMenu;

	private static readonly string[] _resourceNames = { "Wood", "Stone", "Magic", "Food" };

	private static readonly string[] _resourceIcons =
	{
		"res://Graphics/wood_icon.png",
		"res://Graphics/stone_icon.png",
		"res://Graphics/magic_icon.png",
		"res://Graphics/food_icon.png"
	};

	private int _deckCount;
	private int _handCount;
	private Tween _messageTween;

	public CardHand CardHand => GetNodeOrNull<CardHand>("HBoxContainer/CardHandSection/CardHand");

	public int Resources => ResourceManager.Magic;

	public int DeckCount => _deckCount;

	public int HandCount => _handCount;

	public int MessageVersion { get; private set; }

	public override void _Ready()
	{
		instance = this;
		ProcessMode = ProcessModeEnum.Always;
		ResourceManager.Reset();

		/*
		PauseButton ??= GetNode<Button>("BottomPanel/HBoxContainer/InteractiveButtons/VBoxContainer/Pause");
		ShopButton ??= GetNode<Button>("BottomPanel/HBoxContainer/InteractiveButtons/VBoxContainer/Shop");
		PassTurnButton ??= GetNode<Button>("BottomPanel/HBoxContainer/InteractiveButtons/VBoxContainer/PassTurn");
		DeckLabel ??= GetNode<RichTextLabel>("BottomPanel/HBoxContainer/Stats/HBoxContainer/deck");
		ResourcesLabel ??= GetNode<RichTextLabel>("BottomPanel/HBoxContainer/Stats/HBoxContainer/resources");
		*/
		ResourcesLabel.BbcodeEnabled = true;
		CardMessageLabel ??= GetNodeOrNull<Label>("CardMessage");

		PauseMenu ??= GetNodeOrNull<PauseMenu>("PauseMenu");
		ShopMenu ??= GetNodeOrNull<ShopMenu>("ShopMenu");

		if (PauseButton != null)
		{
			PauseButton.Pressed += OnPauseButtonPressed;
		}

		if (ShopButton != null)
		{
			ShopButton.Pressed += OnShopButtonPressed;
		}

		if(PassTurnButton != null)
		{
			PassTurnButton.Pressed += OnPassTurnButtonPressed;
		}

		UpdateResourcesUI();
		UpdateDeckUI();

		if (CardMessageLabel != null)
		{
			CardMessageLabel.Text = string.Empty;
			CardMessageLabel.Modulate = new Color(1f, 1f, 1f, 0f);
		}
	}

	private void OnPauseButtonPressed()
	{
		PauseMenu?.TogglePause();
	}

	private void OnShopButtonPressed()
	{
		ShopMenu?.OpenShop();
	}

	private void OnPassTurnButtonPressed()
	{
		// Send signal to do stuff
		TurnManager.instance.OnPassTurnPressed();
	}

	public void SetTimeOfDayLabel(string s)
	{
		TimeOfDay.Text = "Time Of Day: "+ s;
	}

	public void UpdateResourcesUI()
	{
		if (ResourcesLabel == null)
		{
			return;
		}

		List<string> parts = new();

		for (int i = 0; i < _resourceNames.Length; i++)
		{
			ResourceManager.ResourceType type = (ResourceManager.ResourceType)i;
			parts.Add(ResourceManager.Get(type) + " : [img=24x24]" + _resourceIcons[i] + "[/img] " + _resourceNames[i]);
		}

		ResourcesLabel.Text = string.Join("\n", parts);
	}

	public void UpdateDeckUI()
	{
		if (DeckLabel == null)
		{
			return;
		}

		DeckLabel.Text = _handCount >= 0
			? $"Deck: {_deckCount} | Hand: {_handCount}"
			: $"Deck: {_deckCount}";
	}

	public void AddResources(int amount)
	{
		ResourceManager.Add(ResourceManager.ResourceType.Magic, amount);
		UpdateResourcesUI();
	}

	public bool TrySpendResources(int amount)
	{
		if (!ResourceManager.TrySpend(ResourceManager.ResourceType.Magic, amount))
		{
			return false;
		}

		UpdateResourcesUI();
		return true;
	}

	public void SetResources(int amount)
	{
		ResourceManager.Set(ResourceManager.ResourceType.Magic, amount);
		UpdateResourcesUI();
	}

	public void SetDeckCount(int deckCount, int handCount = -1)
	{
		_deckCount = Mathf.Max(0, deckCount);
		_handCount = handCount;
		UpdateDeckUI();
	}

	public int GiveCards(int amount)
	{
		CardHand hand = CardHand;

		if (hand == null || ShopMenu == null || amount <= 0)
		{
			return 0;
		}

		int deckBefore = hand.DeckCount;
		int addedToHand = hand.GiveCards(amount, ShopMenu.AvailableCardPool);
		int addedToDeck = hand.DeckCount - deckBefore;
		int total = addedToHand + addedToDeck;

		if (total > 0)
		{
			if (addedToHand > 0 && addedToDeck > 0)
			{
				ShowMessage("Drew " + addedToHand + " " + CardWord(addedToHand) + ". " + addedToDeck + " went to deck.");
			}
			else if (addedToHand > 0)
			{
				ShowMessage("Drew " + addedToHand + " " + CardWord(addedToHand) + ".");
			}
			else
			{
				ShowMessage(addedToDeck + " " + CardWord(addedToDeck) + " went to deck.");
			}
		}

		return total;
	}

	public int GiveCardsForTurn(int amount)
	{
		return GiveCards(amount);
	}

	private static string CardWord(int count)
	{
		return count == 1 ? "card" : "cards";
	}

	public void ShowMessage(string message, float holdSeconds = 2.2f)
	{
		MessageVersion++;

		if (CardMessageLabel == null)
		{
			GD.Print("[HUD] " + message);
			return;
		}

		_messageTween?.Kill();

		CardMessageLabel.Text = message;
		CardMessageLabel.Modulate = new Color(1f, 1f, 1f, 1f);

		_messageTween = CreateTween();
		_messageTween.TweenInterval(Mathf.Max(0.1f, holdSeconds));
		_messageTween.TweenProperty(CardMessageLabel, "modulate:a", 0f, 0.35f);
		_messageTween.TweenCallback(Callable.From(() =>
		{
			if (CardMessageLabel != null)
			{
				CardMessageLabel.Text = string.Empty;
			}
		}));
	}
}
