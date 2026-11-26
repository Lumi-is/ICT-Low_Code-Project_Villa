using Godot;

namespace ProjectVilla.Cards;

public partial class CardView : Control
{
	public Card Card;
	public CardHand Hand;
	public bool Dragging { get; private set; }

	private Label _title;
	private Label _woodCost;
	private Label _stoneCost;
	private Label _magicCost;
	private Label _foodCost;
	private Label _description;
	private Label _category;
	private ColorRect _icon;
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
		_icon = GetNode<ColorRect>("%IconPanel");
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

		Resized += SetPivot;
		SetPivot();
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;

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

	public void BeginDrag()
	{
		Dragging = true;
		ZIndex = 100;
		Scale = new Vector2(1.05f, 1.05f);
	}

	public void MoveDrag(Vector2 screenPosition, Vector2 grabOffset)
	{
		GlobalPosition = screenPosition - grabOffset;
	}

	public void EndDrag()
	{
		Dragging = false;
		ZIndex = 0;
		Scale = Vector2.One;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse
			&& mouse.ButtonIndex == MouseButton.Left
			&& mouse.Pressed)
		{
			Interactor.Instance?.BeginCardDrag(this, mouse.GlobalPosition);
			AcceptEvent();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (!Dragging)
		{
			return;
		}

		if (@event is InputEventMouseMotion motion)
		{
			Interactor.Instance?.MoveCardDrag(motion.GlobalPosition);
			GetViewport().SetInputAsHandled();
			return;
		}

		if (@event is InputEventMouseButton mouse
			&& mouse.ButtonIndex == MouseButton.Left
			&& !mouse.Pressed)
		{
			GetViewport().SetInputAsHandled();
			Interactor.Instance?.EndCardDrag();
		}
	}

	private void SetPivot()
	{
		PivotOffset = Size * 0.5f;
	}

	private void OnMouseEntered()
	{
		if (!Dragging)
		{
			Scale = new Vector2(1.05f, 1.05f);
			ZIndex = 10;
		}
	}

	private void OnMouseExited()
	{
		if (!Dragging)
		{
			Scale = Vector2.One;
			ZIndex = 0;
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