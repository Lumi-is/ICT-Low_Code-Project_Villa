using Godot;
using Grid;
using ProjectVilla.Cards;

[GlobalClass]
public partial class BuildCard : Card
{
	[Export] public string path = "res://Scenes/DefaultBuilding.tscn";

	public override bool TryPlay(CardPlayController game, Vector2 playPos)
	{
		Tile target = WorldGridManager.GetTileAtWorldPosition(playPos);

		if (target == null)
		{
			game.Hud?.ShowMessage("Drop the card on a tile.");
			return false;
		}

		if (!WorldGridManager.IsTileFree(target))
		{
			game.Hud?.ShowMessage("That tile is occupied.");
			return false;
		}

		return WorldGridManager.InstantiateObject(playPos, path, target);
	}
}
