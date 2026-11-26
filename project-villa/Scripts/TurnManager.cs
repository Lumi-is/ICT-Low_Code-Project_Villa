using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TurnManager : Node
{
	public static TurnManager instance;
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


	public static List<Enemy> enemyList = new();
	public static List<Troop> troopList = new();


	[Export] public Interactor Interactor;
	[Export] public Hud HUD;
	[Export] public int CardsPerTurn = 2;

	public override void _Ready()
	{
		instance = this;
	}


	public string[] turnPhases =
	[
		"playerStartPhase","playerActionPhase","enemyStartPhase","enemyActionPhase"
	];

	public string currentPhase = "playerStartPhase";

	private bool isTimer;

	// Summary:
	// Boolean method which attempts to start a timer. Returns true if timer could be started, and false is it couldn't. 
	// Only one timer can exist at a time.
	private bool StartTimer(Action doAction, int secondAmount)
	{
		if (isTimer)
		{
			return false;
		}

		isTimer = true;

		SceneTreeTimer timer = GetTree().CreateTimer(secondAmount);
		timer.Timeout += () =>
		{
			isTimer = false;
			doAction();
		};

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
				Hud.instance.SetTimeOfDayLabel("Dusk");

				if(StartTimer(IncrementPhase, 3))
				{
					foreach(Troop troop in troopList)
					{
						troop.hasMoved = false;
					}

					Hud hud = HUD != null ? HUD : Hud.instance;
					hud?.GiveCardsForTurn(CardsPerTurn);
				}
				canvasModulate.Color = canvasModulate.Color.Lerp(duskColor, 0.01f);
				break;

			case "playerActionPhase":
				// What to do
				Hud.instance.SetTimeOfDayLabel("Midnight");

				canvasModulate.Color = canvasModulate.Color.Lerp(midNightColor, 0.01f);

				// Increments phase when player presses "pass turn" 
				break;

			case "enemyStartPhase":
				// What to do
				Hud.instance.SetTimeOfDayLabel("Morning");

				if(StartTimer(IncrementPhase, 3))
				{
					// Do stuff thats only done once = Aka. animations.
				}
				canvasModulate.Color = canvasModulate.Color.Lerp(morningColor, 0.01f);
				break;

			case "enemyActionPhase":
				// What to do
				Hud.instance.SetTimeOfDayLabel("Noon");

				if(enemyList.Count > 0)
				{
					for(int i = 0; i < enemyList.Count; i++)
					{
						Enemy enemy = enemyList[i];
						enemy.isInPlay = true; 

						while (enemy.isInPlay)
						{
							if(StartTimer(enemy.IncrementCurrentAction,1))
							{
								enemy.DoCurrentAction();
							}
						}

						if(i == enemyList.Count - 1)
						{
							Hud.instance.ShowMessage("All enemy actions complete. Forwarding time...");
							StartTimer(IncrementPhase,1);
						}
					}
				} else
				{
					Hud.instance.ShowMessage("No enemies nearby. Forwarding time...");
					StartTimer(IncrementPhase, 3);
				}
				// Do stuff thats only done once
				

				// WIP, add enemies to enemyList as they are originally spawned in

				// Go through every enemy, do their actions one by one and move to the next. When all enemies have done their actions, this phase ends. 
				
				canvasModulate.Color = canvasModulate.Color.Lerp(noonColor, 0.01f);

				
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
