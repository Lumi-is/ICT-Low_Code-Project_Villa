using Godot;

public partial class OptionsMenu : CanvasLayer
{
	[Signal]
	public delegate void ClosedEventHandler();

	[Export] private HSlider _sfxSlider;
	[Export] private Button _creditsButton;
	[Export] private Button _backButton;

	[Export(PropertyHint.File, "*.tscn")]
	public string CreditsScenePath = "res://Scenes/credits_menu.tscn";

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		_sfxSlider ??= GetNode<HSlider>("%SfxSlider");
		_creditsButton ??= GetNode<Button>("%CreditsButton");
		_backButton ??= GetNode<Button>("%BackButton");

		_sfxSlider.ValueChanged += OnSfxVolumeChanged;
		_creditsButton.Pressed += OnCreditsPressed;
		_backButton.Pressed += OnBackPressed;

		int bus = AudioServer.GetBusIndex("Sound effects");
		if (bus >= 0)
		{
			_sfxSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(bus));
		}
	}

	private void OnSfxVolumeChanged(double value)
	{
		int bus = AudioServer.GetBusIndex("Sound effects");
		if (bus < 0)
		{
			return;
		}

		float db = value > 0.0 ? Mathf.LinearToDb((float)value) : -60f;
		AudioServer.SetBusVolumeDb(bus, db);
	}

	private void OnCreditsPressed()
	{
		PackedScene scene = GD.Load<PackedScene>(CreditsScenePath);
		if (scene == null)
		{
			return;
		}

		CreditsMenu credits = scene.Instantiate<CreditsMenu>();
		credits.Closed += OnCreditsClosed;

		Hide();
		AddSibling(credits);
	}

	private void OnCreditsClosed()
	{
		Show();
	}

	private void OnBackPressed()
	{
		EmitSignal(SignalName.Closed);
		QueueFree();
	}
}