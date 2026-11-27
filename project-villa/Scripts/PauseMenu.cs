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

	[Export] private Button _optionsButton;

	[Export(PropertyHint.File, "*.tscn")]
	public string OptionsScenePath = "res://Scenes/OptionsMenu.tscn";

	private bool _optionsOpen;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		ResumeButton ??= GetNode<Button>("UI/MenuButtons/ResumeButton");
		QuitButton ??= GetNode<Button>("UI/MenuButtons/QuitButton");

		ResumeButton.Pressed += OnResumeButtonPressed;
		QuitButton.Pressed += OnQuitButtonPressed;
		_optionsButton ??= GetNode<Button>("UI/MenuButtons/OptionsButton");
		_optionsButton.Pressed += OnOptionsPressed;

		Hide();
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

		_optionsOpen = true;
		GetNode<Control>("UI").Hide();
		AddSibling(options);
	}

	private void OnOptionsClosed()
	{
		_optionsOpen = false;
		GetNode<Control>("UI").Show();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (_optionsOpen)
		{
			return;
		}

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
