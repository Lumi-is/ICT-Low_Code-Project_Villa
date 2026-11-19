using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class Hud : CanvasLayer
{
	[ExportGroup("UI References")]
	[Export] public Button PauseButton;
	[Export] public Button ShopButton;
	[Export] public RichTextLabel DeckLabel;
	[Export] public RichTextLabel ResourcesLabel;
	[Export] public Label CardMessageLabel;

	[ExportGroup("Menus")]
	[Export] public PauseMenu PauseMenu;
	[Export] public ShopMenu ShopMenu;

private readonly List<(string Name, int Amount)> _resourceList = new()
{
	("Wood", 100),
	("Stone", 100),
	("Magic", 100),
	("Food", 100),

};

		private static readonly Dictionary<string, string> _resourceIcons = new()
	{
		{ "Wood",  "res://Graphics/building_material_icon.png" },
		{ "Stone", "res://Graphics/building_material_icon.png" },
		{ "Magic", "res://Graphics/magic_icon.png" },
		{ "Food",  "res://Graphics/food_icon.png" },
	};

	private int _resourceCount = 100;
	private int _deckCount;
	private int _handCount;
	private Tween _messageTween;

	public int Resources => _resourceCount;

	public int DeckCount => _deckCount;

	public int HandCount => _handCount;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		PauseButton ??= GetNode<Button>("BottomPanel/HBoxContainer/InteractiveButtons/VBoxContainer/Pause");
		ShopButton ??= GetNode<Button>("BottomPanel/HBoxContainer/InteractiveButtons/VBoxContainer/Shop");
		DeckLabel ??=  GetNode<RichTextLabel> ("BottomPanel/HBoxContainer/Stats/HBoxContainer/deck");
		ResourcesLabel ??= GetNode<RichTextLabel>("BottomPanel/HBoxContainer/Stats/HBoxContainer/resources");
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
		GD.Print("Pause button clicked");
		PauseMenu?.TogglePause();
	}

	private void OnShopButtonPressed()
	{
		ShopMenu?.OpenShop();
	}

	public void UpdateResourcesUI()
	{
		if (ResourcesLabel == null)
		{
			return;
		}

		var parts = new List<string>();

		foreach (var resource in _resourceList)
		{
			string iconTag = "";

			if (_resourceIcons.TryGetValue(resource.Name, out string iconPath))
			{
				iconTag = $"[img=24x24]{iconPath}[/img] ";
			}

			parts.Add($"{resource.Amount} : {iconTag}");
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
		_resourceCount = Mathf.Max(0, _resourceCount + amount);
		UpdateResourcesUI();
	}

	public bool TrySpendResources(int amount)
	{
		if (amount <= 0)
		{
			return true;
		}

		if (_resourceCount < amount)
		{
			return false;
		}

		_resourceCount -= amount;
		UpdateResourcesUI();
		return true;
	}

	public void SetResources(int amount)
	{
		_resourceCount = Mathf.Max(0, amount);
		UpdateResourcesUI();
	}

	public void SetDeckCount(int deckCount, int handCount = -1)
	{
		_deckCount = Mathf.Max(0, deckCount);
		_handCount = handCount;
		UpdateDeckUI();
	}

	public void ShowCardMessage(string message, float holdSeconds = 2.2f)
	{
		if (CardMessageLabel == null)
		{
			GD.Print($"[HUD] {message}");
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
