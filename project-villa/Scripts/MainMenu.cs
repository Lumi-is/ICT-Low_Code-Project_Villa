using Godot;
using System;

public partial class MainMenu : Control
{
	[ExportGroup("Scene Paths")]
	[Export(PropertyHint.File, "*.tscn")]
	public string GameScenePath = "res://Scenes/TestingScene.tscn";

	[ExportGroup("UI References")]
	[Export] public Button PlayButton;
	[Export] public Button QuitButton;

	[Export] private Button _optionsButton;

[Export(PropertyHint.File, "*.tscn")]
public string OptionsScenePath = "res://Scenes/OptionsMenu.tscn";

	public override void _Ready()
	{
		PlayButton ??= GetNode<Button>("VBoxContainer/StartButton");
		QuitButton ??= GetNode<Button>("VBoxContainer/QuitButton");

		PlayButton.Pressed += OnPlayButtonPressed;
		QuitButton.Pressed += OnQuitButtonPressed;
		_optionsButton ??= GetNode<Button>("VBoxContainer/OptionsButton");
		_optionsButton.Pressed += OnOptionsPressed;
	}

private void OnOptionsPressed()
{
	PackedScene scene = GD.Load<PackedScene>(OptionsScenePath);
	if (scene == null)
	{
		return;
	}

	OptionsMenu options = scene.Instantiate<OptionsMenu>();
	options.Closed += OnOptionsClosed;

	GetNode<Control>("VBoxContainer").Hide();
	GetNode<Control>("GameTitle").Hide();
	AddChild(options);
}

private void OnOptionsClosed()
{
	GetNode<Control>("VBoxContainer").Show();
	GetNode<Control>("GameTitle").Show();
}

	private void OnPlayButtonPressed()
	{
		if (string.IsNullOrEmpty(GameScenePath))
		{
			GameScenePath = "res://Scenes/TestingScene.tscn";
		}

		GetTree().ChangeSceneToFile(GameScenePath);
	}

	private async void OnQuitButtonPressed()
	{
		await ToSignal(GetTree().CreateTimer(0.2), SceneTreeTimer.SignalName.Timeout);
		GetTree().Quit();
	}
}
