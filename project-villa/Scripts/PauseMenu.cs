using Godot;
using System;

[GlobalClass]
public partial class PauseMenu : CanvasLayer
{
    [ExportGroup("UI References")]
    [Export] public Button ResumeButton;
    [Export] public Button QuitButton;

    // The button labelled "Main Menu" returns to this scene.
    [Export(PropertyHint.File, "*.tscn")]
    public string MainMenuScenePath = "res://Scenes/MainMenu.tscn";

    public override void _Ready()
    {
        // Keep working while the game tree is paused.
        ProcessMode = ProcessModeEnum.Always;

        // Fetch node references based on the PauseMenu.tscn scene tree.
        ResumeButton ??= GetNode<Button>("UI/MenuButtons/ResumeButton");
        QuitButton ??= GetNode<Button>("UI/MenuButtons/QuitButton");

        ResumeButton.Pressed += OnResumeButtonPressed;
        QuitButton.Pressed += OnQuitButtonPressed;

        // Start hidden by default.
        Hide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // Toggle pause menu with the "ui_cancel" action (default is Escape key).
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

    // --- Button Callbacks ---

    private void OnResumeButtonPressed()
    {
        ResumeGame();
    }

    private void OnQuitButtonPressed()
    {
        // Always unpause before changing scenes.
        GetTree().Paused = false;

        if (string.IsNullOrEmpty(MainMenuScenePath))
        {
            MainMenuScenePath = "res://Scenes/MainMenu.tscn";
        }

        GetTree().ChangeSceneToFile(MainMenuScenePath);
    }
}
