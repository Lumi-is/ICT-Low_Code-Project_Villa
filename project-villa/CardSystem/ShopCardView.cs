using Godot;
using System;

namespace ProjectVilla.Cards;

public partial class ShopCardView : Control
{
	public event Action<ShopCardView> BuyRequested;

	public Card Card { get; private set; }

	private Label _title;
	private Label _cost;
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
		_cost = GetNode<Label>("%Cost");
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
		_cost.Text = Card.Cost <= 0 ? "FREE" : Card.Cost.ToString();
		_description.Text = Card.Description;
		_category.Text = Card.Category.ToUpper();
		_price.Text = Card.ShopPrice <= 0 ? "Price: Free" : "Price: " + Card.ShopPrice;
		_style.BorderColor = Card.AccentColor;
		_icon.Color = Card.AccentColor.Darkened(0.65f);
		_cost.AddThemeColorOverride("font_color", Card.AccentColor.Lightened(0.3f));
	}
}
