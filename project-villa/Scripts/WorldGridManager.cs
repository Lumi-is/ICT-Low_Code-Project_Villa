using System;
using System.Collections.Generic;
using Godot;

namespace Grid
{
	static class WorldGridManager
	{
		public static float tileSize = 128;

		public static Tile highlightedTile; 
		public static List<List<Tile>> groundLevelGrid;
		public static List<List<Tile>> treeLevelGrid;
		public static IBuilding[,] buildingGrid = new IBuilding[512,512];
		public static List<IBuilding> buildingList;

		public static Vector2 GetNearestCell(Vector2 position)
		{
            var cellPos = CalculateSnapPos(position);

			Vector2 nearestCell = new Vector2((cellPos.X - tileSize/2) / tileSize, (cellPos.Y - tileSize/2) / tileSize);

			return nearestCell;
		}

		public static Vector2 CalculateSnapPos(Vector2 position)
		{
			var xPosition = position.X;
			var yPosition = position.Y;
	
            Vector2 cellPos = new();

			var xSign = Math.Sign(xPosition);
			var ySign = Math.Sign(yPosition);

			// X
			// Calculating how far the object is from the nearest cell
            float xOffset = xPosition % tileSize;

			// Calculating how many jumps from origo the nearest cell is
            float xJumpCount = (xPosition - xOffset) / tileSize;

			// Calculating what the desired position of the object should be when adjusted to the grid
            cellPos.X = xJumpCount * tileSize + (xSign * (tileSize/2));


			// Y
			// Calculating how far the object is from the nearest cell
            float yOffset = yPosition % tileSize;

			// Calculating how many jumps from origo the nearest cell is
            float yJumpCount = (yPosition - yOffset) / tileSize;

			// Calculating what the desired position of the object should be when adjusted to the grid
            cellPos.Y = yJumpCount * tileSize + (ySign * (tileSize/2));

			GD.Print("CellPos"+cellPos);

			return cellPos;
		}

		public static void MoveOwner(Node2D someSnappable, Vector2 snapPos)
		{
			Node2D snappable;
			// Iterate upon this later
			if(someSnappable.GetOwner<Node2D>() != null)
			{
				snappable = someSnappable.GetOwner<Node2D>();
			} else
			{
				snappable = someSnappable;
			}
			
			snappable.Position	= snapPos;
		}


		public static bool InstantiateObject(Vector2 pos, string path)
		{
			PackedScene scene;
			scene = ResourceLoader.Load<PackedScene>(path);

			Node2D instantiable = (Node2D)scene.Instantiate();

			if(instantiable is IBuilding)
			{
				highlightedTile.AddChild(instantiable);
				instantiable.GlobalPosition = highlightedTile.GlobalPosition;

				if(TryPlaceBuilding(instantiable.GlobalPosition, (IBuilding)instantiable) != null)
				{
					return true;
				} else
				{
					return false;
				}
				
			} else 
			{
				// For possible non building instantiables
				MoveOwner(instantiable, CalculateSnapPos(pos));
				return true;
			}
		}

		public static IBuilding TryPlaceBuilding(Vector2 pos, IBuilding placeable)
		{
			Vector2 buildGridIndex = new(GetNearestCell(pos).X+256, GetNearestCell(pos).Y+256);
			IBuilding building = buildingGrid[(int)buildGridIndex.X, (int)buildGridIndex.Y];
			if(building == null)
			{
				building = buildingGrid[(int)buildGridIndex.X, (int)buildGridIndex.Y] = placeable;
				return building;
			} else
			{
				return null;
			}
		}
	}
}