using Godot;

public partial class CreditsMenu : CanvasLayer
{
	[Signal]
	public delegate void ClosedEventHandler();

	[Export] private Button _backButton;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		_backButton ??= GetNode<Button>("%BackButton");
		_backButton.Pressed += OnBackPressed;
	}

	private void OnBackPressed()
	{
		EmitSignal(SignalName.Closed);
		QueueFree();
	}
}