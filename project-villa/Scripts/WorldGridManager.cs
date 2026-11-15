using System;
using Godot;

namespace Grid
{
	static class WorldGridManager
	{

		public static Tile[][] groundLevelGrid;
		public static Tile[][] treeLevelGrid;
		public static IBuilding[][] buildingGrid;

		public static Vector2 CalculateSnapPos(Vector2 position)
		{
		    var jumpInterval = 100f;
            var xPosition = position.X;
			var yPosition = position.Y;
	
            Vector2 snapPos = new();

			var xSign = Math.Sign(xPosition);
			var ySign = Math.Sign(yPosition);

			// X
			// Calculating how far the object is from the nearest cell
            float xOffset = xPosition % jumpInterval;

			// Calculating how many jumps from origo the nearest cell is
            float xJumpCount = (xPosition - xOffset) / jumpInterval;

			// Calculating what the desired position of the object should be when adjusted to the grid
            snapPos.X = xJumpCount * jumpInterval + (xSign * (jumpInterval/2));


			// Y
			// Calculating how far the object is from the nearest cell
            float yOffset = yPosition % jumpInterval;

			// Calculating how many jumps from origo the nearest cell is
            float yJumpCount = (yPosition - yOffset) / jumpInterval;

			// Calculating what the desired position of the object should be when adjusted to the grid
            snapPos.Y = yJumpCount * jumpInterval + (ySign * (jumpInterval/2));

			return snapPos;
		}

		public static void SnapToGrid(Node2D someSnappable, Vector2 snapPos)
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
	}
}