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
		public static Building[,] buildingGrid = new Building[512,512];
		public static Troop[,] troopGrid = new Troop[512,512];
		public static List<Building> buildingList = new();

		private static readonly Dictionary<Vector2I, Tile> _tiles = new();

		public static int[] GetNearestCell(Vector2 position)
		{
            var cellPos = CalculateSnapPos(position);

			int[] nearestCell = {(int)((cellPos.X - tileSize/2) / tileSize), (int)((cellPos.Y - tileSize/2) / tileSize)};

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

			//GD.Print("CellPos"+cellPos);

			return cellPos;
		}

		public static void Reset()
		{
			highlightedTile = null;
			_tiles.Clear();
			Array.Clear(buildingGrid, 0, buildingGrid.Length);
		}

		public static void RegisterTile(Tile tile)
		{
			if (tile == null)
			{
				return;
			}

			Vector2I index = GetGridIndex(tile.GlobalPosition);

			if (IsValidIndex(index))
			{
				_tiles[index] = tile;
			}
		}

		public static Tile GetTileAtWorldPosition(Vector2 worldPosition)
		{
			Vector2I index = GetGridIndex(worldPosition);
			_tiles.TryGetValue(index, out Tile tile);
			return tile;
		}

		public static bool IsTileFree(Tile tile)
		{
			if (tile == null)
			{
				return false;
			}

			Vector2I index = GetGridIndex(tile.GlobalPosition);
			return IsValidIndex(index) && buildingGrid[index.X, index.Y] == null;
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


		public static bool InstantiateObject(Vector2 pos, string path, Tile targetTile)
		{

			if (targetTile == null)
			{
				return false;
			}

			PackedScene scene = ResourceLoader.Load<PackedScene>(path);

			if (scene == null)
			{
				return false;
			}

			Node2D instantiable = (Node2D)scene.Instantiate();

			if(instantiable is Building structure)
			{
				if(TryPlaceBuilding(targetTile.GlobalPosition, (Building)instantiable) != null)
				{
					targetTile.AddChild(instantiable);
					
					instantiable.GlobalPosition = targetTile.GlobalPosition;

					structure.OnBuild();
					
					return true;
				} else
				{
					return false;
				}
				
			} else 
			{
				// For non building instantiables
				MoveOwner(instantiable, CalculateSnapPos(pos));
				return true;
			}
		}

		public static Building TryPlaceBuilding(Vector2 pos, Building placeable)
		{
			Vector2 buildGridIndex = new(GetNearestCell(pos)[0]+256, GetNearestCell(pos)[1]+256);
			Building building = buildingGrid[(int)buildGridIndex.X, (int)buildGridIndex.Y];
			if(building == null)
			{
				building = buildingGrid[(int)buildGridIndex.X, (int)buildGridIndex.Y] = placeable;
				buildingList.Add(building);
				return building;
			} else
			{
				return null;
			}
		}

		public static Type GetCellContentsType(Vector2 pos)
		{
			var nearestCell = GetNearestCell(pos);

			if(buildingGrid[nearestCell[0] + 256, nearestCell[1] + 256] != null)
			{
				return typeof(Building);
			} else if(troopGrid[nearestCell[0] + 256, nearestCell[1] + 256] != null)
			{
				return typeof(Troop);
			} else
			{
				return null;
			}
		}

		private static Vector2I GetGridIndex(Vector2 worldPosition)
		{
			Vector2 cell = new(GetNearestCell(worldPosition)[0],GetNearestCell(worldPosition)[1]);
			return new Vector2I(Mathf.RoundToInt(cell.X) + 256, Mathf.RoundToInt(cell.Y) + 256);
		}

		private static bool IsValidIndex(Vector2I index)
		{
			return index.X >= 0 && index.X < buildingGrid.GetLength(0)
				&& index.Y >= 0 && index.Y < buildingGrid.GetLength(1);
		}
	}
}