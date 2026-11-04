using Godot;
using System;

[GlobalClass]
public partial class PauseMenu : CanvasLayer
{
	[ExportGroup("UI References")]
	[Export] public Button ResumeButton;
	[Export] public Button QuitButton;

	[Export(PropertyHint.File, "*.tscn")]
	public string MainMenuScenePath = "res://Scenes/MainMenu.tscn";

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		ResumeButton ??= GetNode<Button>("UI/MenuButtons/ResumeButton");
		QuitButton ??= GetNode<Button>("UI/MenuButtons/QuitButton");

		ResumeButton.Pressed += OnResumeButtonPressed;
		QuitButton.Pressed += OnQuitButtonPressed;

		Hide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			TogglePause();
		}
	}

	public void TogglePause()
	{
		if (Visible)
		{
			ResumeGame();
		}
		else
		{
			GetTree().Paused = true;
			Show();
		}
	}

	public void ResumeGame()
	{
		GetTree().Paused = false;
		Hide();
	}

	private void OnResumeButtonPressed()
	{
		ResumeGame();
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Paused = false;

		if (string.IsNullOrEmpty(MainMenuScenePath))
		{
			MainMenuScenePath = "res://Scenes/MainMenu.tscn";
		}

		GetTree().ChangeSceneToFile(MainMenuScenePath);
	}
}
