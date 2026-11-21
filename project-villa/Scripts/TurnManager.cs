using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

public partial class TurnManager : Node
{
	// Player's turn happens during the "night" in the world, apply simple filter for showcase purposes and adjust it's opacity to match the state of the game
	// Enemies' turns happen during the "daytime" in the world. Remove filter during it

	// 1. Give cards to player according to town centre level. Note: How to make this understandable to player? Animation?
	// 2. Allow player to play cards, buy cards from shop, move troops, discard cards from hand, etc. 
	// 3. Move camera to nearby enemy positions, do enemy actions (one enemy at a time). If none are nearby, go to next turn. 

	// Called when the node enters the scene tree for the first time.

	// canvasModulate to modify scene color
	[Export] public CanvasModulate canvasModulate; 

	[Export] public Color duskColor;
	[Export] public Color midNightColor;
	[Export] public Color morningColor;
	[Export] public Color noonColor;

	public string[] turnPhases =
    [
        "playerStartPhase","playerActionPhase","enemyStartPhase","enemyActionPhase"
	];

	public string currentPhase = "playerStartPhase";

    public override void _Ready()
    {
        timer();
    }

	private void timer()
	{
		Task.Delay(TimeSpan.FromSeconds(5f)).ContinueWith(_ =>
		{
			// executed every 5 seconds
			GD.Print("meoww");
			var nextPhaseIndex = Array.FindIndex(turnPhases, IsCurrentPhase) + 1;

			if(nextPhaseIndex < turnPhases.Length)
			{
				currentPhase = turnPhases[nextPhaseIndex];
			} else
			{
				currentPhase = turnPhases[0];
			}
			
			timer();
		});
	}

	private bool IsCurrentPhase(string s)
	{
		if(s == currentPhase) return true;

		return false;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		switch(currentPhase)
        {
            case "playerStartPhase":
				// What to do
				canvasModulate.Color = canvasModulate.Color.Lerp(duskColor, 0.01f);
				
				break;

			case "playerActionPhase":
				// What to do
				canvasModulate.Color = canvasModulate.Color.Lerp(midNightColor, 0.01f);
				break;

			case "enemyStartPhase":
				// What to do
				canvasModulate.Color = canvasModulate.Color.Lerp(morningColor, 0.01f);
				break;

			case "enemyActionPhase":
				// What to do
				canvasModulate.Color = canvasModulate.Color.Lerp(noonColor, 0.01f);
				break;
		}
	}
}
