using Godot;
using Grid;
using ProjectVilla.Cards;

[GlobalClass]
public partial class BuildCard : Card
{
	[Export] public string path = "res://Scenes/DefaultBuilding.tscn";

	public override bool TryPlay(CardPlayController game, Vector2 playPos)
	{
		if (WorldGridManager.highlightedTile == null)
		{
			game.Hud?.ShowMessage("Hover over a tile first.");
			return false;
		}

		return WorldGridManager.InstantiateObject(playPos, path);
	}
}
