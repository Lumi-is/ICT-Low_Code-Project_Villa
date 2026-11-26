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

		private static readonly Dictionary<Vector2I, Tile> _tiles = new();

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

			float xOffset = xPosition % tileSize;
			float xJumpCount = (xPosition - xOffset) / tileSize;
			cellPos.X = xJumpCount * tileSize + (xSign * (tileSize/2));

			float yOffset = yPosition % tileSize;
			float yJumpCount = (yPosition - yOffset) / tileSize;
			cellPos.Y = yJumpCount * tileSize + (ySign * (tileSize/2));

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

			if(someSnappable.GetOwner<Node2D>() != null)
			{
				snappable = someSnappable.GetOwner<Node2D>();
			}
			else
			{
				snappable = someSnappable;
			}

			snappable.Position = snapPos;
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

			if(instantiable is IBuilding building)
			{
				if(TryPlaceBuilding(targetTile.GlobalPosition, building) == null)
				{
					return false;
				}

				targetTile.AddChild(instantiable);
				instantiable.GlobalPosition = targetTile.GlobalPosition;
				return true;
			}

			MoveOwner(instantiable, CalculateSnapPos(pos));
			return true;
		}

		public static IBuilding TryPlaceBuilding(Vector2 pos, IBuilding placeable)
		{
			Vector2I index = GetGridIndex(pos);

			if (!IsValidIndex(index) || buildingGrid[index.X, index.Y] != null)
			{
				return null;
			}

			IBuilding building = buildingGrid[index.X, index.Y] = placeable;
			return building;
		}

		private static Vector2I GetGridIndex(Vector2 worldPosition)
		{
			Vector2 cell = GetNearestCell(worldPosition);
			return new Vector2I(Mathf.RoundToInt(cell.X) + 256, Mathf.RoundToInt(cell.Y) + 256);
		}

		private static bool IsValidIndex(Vector2I index)
		{
			return index.X >= 0 && index.X < buildingGrid.GetLength(0)
				&& index.Y >= 0 && index.Y < buildingGrid.GetLength(1);
		}
	}
}
