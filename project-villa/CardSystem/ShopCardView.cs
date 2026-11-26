using Godot;
using System;

namespace ProjectVilla.Cards;

public partial class ShopCardView : Control
{
	public event Action<ShopCardView> BuyRequested;

	public Card Card { get; private set; }

	private Label _title;
	private Label _woodCost;
	private Label _stoneCost;
	private Label _magicCost;
	private Label _foodCost;
	private Label _description;
	private Label _category;
	private Label _price;
	private ColorRect _icon;
	private Button _buyButton;
	private PanelContainer _panel;
	private StyleBoxFlat _style;

	public override void _Ready()
	{
		_title = GetNode<Label>("%Title");
		_woodCost = GetNode<Label>("%Cost");
		_stoneCost = GetNode<Label>("%StoneNumber");
		_magicCost = GetNode<Label>("%MagicNumber");
		_foodCost = GetNode<Label>("%FoodNumber");
		_description = GetNode<Label>("%Description");
		_category = GetNode<Label>("%Category");
		_price = GetNode<Label>("%Price");
		_icon = GetNode<ColorRect>("%IconPanel");
		_buyButton = GetNode<Button>("%BuyButton");
		_panel = GetNode<PanelContainer>("%Panel");

		_style = new StyleBoxFlat
		{
			BgColor = new Color("#141b26"),
			CornerRadiusTopLeft = 14,
			CornerRadiusTopRight = 14,
			CornerRadiusBottomLeft = 14,
			CornerRadiusBottomRight = 14,
			BorderWidthLeft = 2,
			BorderWidthTop = 2,
			BorderWidthRight = 2,
			BorderWidthBottom = 2,
			BorderColor = new Color("#e0b85a"),
		};
		_panel.AddThemeStyleboxOverride("panel", _style);

		_buyButton.Pressed += OnBuyPressed;
		UpdateVisuals();
	}

	public void SetCard(Card card)
	{
		Card = card;

		if (IsNodeReady())
		{
			UpdateVisuals();
		}
	}

	private void OnBuyPressed()
	{
		if (Card != null)
		{
			BuyRequested?.Invoke(this);
		}
	}

	private void UpdateVisuals()
	{
		if (Card == null)
		{
			return;
		}

		_title.Text = Card.DisplayName;
		SetCostLabel(_woodCost, Card.WoodCost, Card.AccentColor);
		SetCostLabel(_stoneCost, Card.StoneCost, Card.AccentColor);
		SetCostLabel(_magicCost, Card.MagicCost, Card.AccentColor);
		SetCostLabel(_foodCost, Card.FoodCost, Card.AccentColor);
		_description.Text = Card.Description;
		_category.Text = Card.Category.ToUpper();
		_price.Text = Card.ShopPrice <= 0 ? "Price: Free" : "Price: " + Card.ShopPrice + " Magic";
		_style.BorderColor = Card.AccentColor;
		_icon.Color = Card.AccentColor.Darkened(0.65f);
	}

	private static void SetCostLabel(Label label, int amount, Color accent)
	{
		label.Text = amount > 0 ? amount.ToString() : "-";

		if (amount > 0)
		{
			label.AddThemeColorOverride("font_color", accent.Lightened(0.3f));
		}
	}
}
