using Godot;

namespace ProjectVilla.Cards;

public partial class CardView : Control
{
	public Card Card;
	public CardHand Hand;
	public bool Dragging { get; private set; }

	private const float DragDistance = 6f;

	private Label _title;

	private Label _description;
	private Label _category;
	private ColorRect _icon;
	private PanelContainer _panel;
	private StyleBoxFlat _style;
	private Label _woodCost;
	private Label _stoneCost;
	private Label _magicCost;
	private Label _foodCost;

	private bool _pressed;
	private Vector2 _pressPosition;
	private Vector2 _grabOffset;

	public override void _Ready()
	{
		_title = GetNode<Label>("%Title");
		_woodCost  = GetNode<Label>("%Cost");
		_stoneCost = GetNode<Label>("%StoneNumber");
		_magicCost = GetNode<Label>("%MagicNumber");
		_foodCost  = GetNode<Label>("%FoodNumber");

		
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

		SetProcessInput(false);
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

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse
			&& mouse.ButtonIndex == MouseButton.Left
			&& mouse.Pressed)
		{
			_pressed = true;
			_pressPosition = mouse.GlobalPosition;
			_grabOffset = mouse.GlobalPosition - GlobalPosition;
			SetProcessInput(true);
			AcceptEvent();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (!_pressed)
		{
			return;
		}

		if (@event is InputEventMouseMotion motion)
		{
			if (!Dragging && motion.GlobalPosition.DistanceTo(_pressPosition) >= DragDistance)
			{
				Dragging = true;
				ZIndex = 100;
				Scale = new Vector2(1.05f, 1.05f);
			}

			if (Dragging)
			{
				GlobalPosition = motion.GlobalPosition - _grabOffset;
				GetViewport().SetInputAsHandled();
			}

			return;
		}

		if (@event is InputEventMouseButton mouse
			&& mouse.ButtonIndex == MouseButton.Left
			&& !mouse.Pressed)
		{
			_pressed = false;
			SetProcessInput(false);
			GetViewport().SetInputAsHandled();

			if (Dragging)
			{
				Dragging = false;
				Scale = Vector2.One;
				Hand?.PlayCard(this);
			}
		}
	}

	public override void _Process(double delta)
	{
		if (_pressed && !Input.IsMouseButtonPressed(MouseButton.Left))
		{
			_pressed = false;
			Dragging = false;
			Scale = Vector2.One;
			ZIndex = 0;
			SetProcessInput(false);
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
		_woodCost.Text  = Card.woodCost  > 0 ? Card.woodCost.ToString()  : "-";
		_stoneCost.Text = Card.stoneCost > 0 ? Card.stoneCost.ToString() : "-";
		_magicCost.Text = Card.magicCost > 0 ? Card.magicCost.ToString() : "-";
		_foodCost.Text  = Card.foodCost  > 0 ? Card.foodCost.ToString()  : "-";
		_description.Text = Card.Description;
		_category.Text = Card.Category.ToUpper();
		_style.BorderColor = Card.AccentColor;
		_icon.Color = Card.AccentColor.Darkened(0.65f);
		foreach (Label label in new[] { _woodCost, _stoneCost, _magicCost, _foodCost })
{
    label.AddThemeColorOverride("font_color", Card.AccentColor.Lightened(0.3f));
}
	}
}
