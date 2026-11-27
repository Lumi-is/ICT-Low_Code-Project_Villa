using Godot;
using System;
using ProjectVilla.Cards;
using Global;

[GlobalClass]
public partial class ExplosiveSpell : Card
{
    [Export] int explosionRadius;
    public override bool TryPlay(CardPlayController game, Vector2 playPos)
    {
        // Play animation 

        // Destroy object at position 
        Vector2I nearestCell = WorldManager.GetNearestCell(playPos);

        if(WorldManager.GetCellContentsType(playPos) == typeof(Building))
        {
            Building building = WorldManager.buildingGrid[nearestCell.X+256,nearestCell.Y+256];

            WorldManager.buildingGrid[nearestCell.X+256,nearestCell.Y+256] = null;

            building.Destruct();
            return true;
        } 
        else if(WorldManager.GetCellContentsType(playPos) == typeof(Troop))
        {
            Troop troop = WorldManager.troopGrid[nearestCell.X+256,nearestCell.Y+256];

            WorldManager.troopGrid[nearestCell.X+256,nearestCell.Y+256] = null;

            troop.Destruct();
            return true;
        } else
        {
             return false;
        }
    }
} 
