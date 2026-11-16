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

	public override void _Ready()
	{
		PlayButton ??= GetNode<Button>("VBoxContainer/StartButton");
		QuitButton ??= GetNode<Button>("VBoxContainer/QuitButton");

		PlayButton.Pressed += OnPlayButtonPressed;
		QuitButton.Pressed += OnQuitButtonPressed;
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
