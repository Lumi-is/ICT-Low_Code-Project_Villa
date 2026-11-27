using Godot;
using Global;
using ProjectVilla.Cards;

[GlobalClass]
public partial class TroopCard : Card
{
	[Export] public string path = "res://Scenes/Troops/TroopRook.tscn";

	public override bool TryPlay(CardPlayController game, Vector2 playPos)
	{
		Tile target = WorldManager.GetTileAtWorldPosition(playPos);

		if (target == null)
		{
			game.Hud?.ShowMessage("Drop the card on a tile.");
			return false;
		}

		if (!WorldManager.IsTileFree(target))
		{
			game.Hud?.ShowMessage("That tile is occupied.");
			return false;
		}

		return WorldManager.InstantiateObject(playPos, path, target);
	}
}
