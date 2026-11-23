using System;
using Godot;
public partial class Enemy : Node
{
	// When called from turnManager, needs to go through all AI actions and then pass onto next enemy.


	// List of different enemy actions which the AI goes through during each turn
	public string[] enemyActions = 
	{
		"startTurn","playCards","moveTroops"
	};

	// Which of the aforementioned actions is currently playing
	string currentAction;

	// Whether or not all actions are done
	public bool isInPlay;


	// Does current action
	public void DoCurrentAction()
	{
		switch (currentAction)
		{
			case "startTurn":
				isInPlay = true;
				break;
		}
	}

	// Tests for Array.FindIndex
	private bool IsCurrentAction(string s)
	{
		if(s == currentAction) return true;

		return false;
	}

	// Increments current action; takes new one from enemyActions array, if none left marks all actions as done
	public void IncrementCurrentAction()
	{
		var nextActionIndex = Array.FindIndex(enemyActions, IsCurrentAction) + 1;

		if(nextActionIndex < enemyActions.Length)
		{
			currentAction = enemyActions[nextActionIndex];
		} else
		{
			MarkAllActionsAsDone();
		}
	}

	// I wonder what this does

	public void MarkAllActionsAsDone()
	{
		isInPlay = false;
	}
}
