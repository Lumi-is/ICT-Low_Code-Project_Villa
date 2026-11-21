using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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


	public static List<Enemy> enemyList;


	[Export] public Interactor Interactor;
	[Export] public Node HUD;


	public string[] turnPhases =
    [
        "playerStartPhase","playerActionPhase","enemyStartPhase","enemyActionPhase"
	];

	public string currentPhase = "playerStartPhase";

	private bool isTimer;

	private bool StartTimer(Action doAction, int secondAmount)
	{
		if(isTimer) return false;

		isTimer = true;
		Task.Delay(TimeSpan.FromSeconds(secondAmount)).ContinueWith(_ => {isTimer = false; doAction();});
		return true;
	}

	private bool IsCurrentPhase(string s)
	{
		if(s == currentPhase) return true;

		return false;
	}

	public void IncrementPhase()
	{
		var nextPhaseIndex = Array.FindIndex(turnPhases, IsCurrentPhase) + 1;

		if(nextPhaseIndex < turnPhases.Length)
		{
			currentPhase = turnPhases[nextPhaseIndex];
		} else
		{
			currentPhase = turnPhases[0];
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch(currentPhase)
        {
            case "playerStartPhase":
				// What to do
				if(StartTimer(IncrementPhase, 1))
				{
					// Do stuff thats only done once = Aka. give cards to player etc.
				}
				canvasModulate.Color = canvasModulate.Color.Lerp(duskColor, 0.01f);
				break;

			case "playerActionPhase":
				// What to do
				canvasModulate.Color = canvasModulate.Color.Lerp(midNightColor, 0.01f);

				// Increment phase when player presses a button
				break;

			case "enemyStartPhase":
				// What to do
				if(StartTimer(IncrementPhase, 1))
				{
					// Do stuff thats only done once = Aka. animations.
				}
				canvasModulate.Color = canvasModulate.Color.Lerp(morningColor, 0.01f);
				break;

			case "enemyActionPhase":
				// What to do

				// WIP, add enemies to enemyList as they are originally spawned in

				// Go through every enemy, do their actions one by one and move to the next. When all enemies have done their actions, this phase ends. 
				foreach(Enemy enemy in enemyList)
				{
					enemy.isInPlay = true; 

					while (enemy.isInPlay)
					{
						if(StartTimer(enemy.IncrementCurrentAction,1))
						{
							enemy.DoCurrentAction();
						}
					}
				}
				canvasModulate.Color = canvasModulate.Color.Lerp(noonColor, 0.01f);

				IncrementPhase();
				break;
		}
	}

	public void OnPassTurnPressed()
	{
		if(currentPhase == "playerActionPhase")
		{
			IncrementPhase();
		}	
	}
}
