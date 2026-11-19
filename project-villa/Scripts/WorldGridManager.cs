using System;
using System.Collections.Generic;
using Godot;

namespace Grid
{
	static class WorldGridManager
	{
		public static float jumpInterval = 100f;
		public static List<List<Tile>> groundLevelGrid;
		public static List<List<Tile>> treeLevelGrid;
		public static IBuilding[,] buildingGrid = new IBuilding[512,512];

		public static Vector2 GetNearestCell(Vector2 position)
		{
			var xPosition = position.X;
			var yPosition = position.Y;
			var xSign = Math.Sign(xPosition);
			var ySign = Math.Sign(yPosition);


            var cellPos = CalculateSnapPos(position);

			Vector2 nearestCell = new Vector2((cellPos.X - jumpInterval/2) / jumpInterval, (cellPos.Y - jumpInterval/2) / jumpInterval);

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
            float xOffset = xPosition % jumpInterval;

			// Calculating how many jumps from origo the nearest cell is
            float xJumpCount = (xPosition - xOffset) / jumpInterval;

			// Calculating what the desired position of the object should be when adjusted to the grid
            cellPos.X = xJumpCount * jumpInterval + (xSign * (jumpInterval/2));


			// Y
			// Calculating how far the object is from the nearest cell
            float yOffset = yPosition % jumpInterval;

			// Calculating how many jumps from origo the nearest cell is
            float yJumpCount = (yPosition - yOffset) / jumpInterval;

			// Calculating what the desired position of the object should be when adjusted to the grid
            cellPos.Y = yJumpCount * jumpInterval + (ySign * (jumpInterval/2));

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


		public static void InstantiateObject(Vector2 pos, Node n, string path)
		{
			PackedScene scene;
			scene = ResourceLoader.Load<PackedScene>(path);

			Node2D instantiable = (Node2D)scene.Instantiate();
			Node parentNode = new();

			if(n.GetChild(0) == null)
			{
				n.AddChild(parentNode);
			} else
			{
				parentNode = n.GetChild(0);
			}
			
			parentNode.AddChild(instantiable);


			// Fix this part later
			// Reason for the WindowSize/2 offset is that card manager system is currently tied to GUI, 
			// which godot seems to handle differently from other objects. If card system is moved from GUI 
			// to a seperate scene, the offset can be deleted. 

			if(instantiable is IBuilding building)
			{
				var xPos = GetNearestCell(pos - DisplayServer.WindowGetSize()/2).X;
				var yPos = GetNearestCell(pos - DisplayServer.WindowGetSize()/2).Y;

				GD.Print(xPos + " , "+yPos);
				buildingGrid[(int)xPos+256,(int)yPos+256] = building;
				MoveOwner(instantiable, CalculateSnapPos(pos - DisplayServer.WindowGetSize()/2));
			} else 
			{
				MoveOwner(instantiable, CalculateSnapPos(pos));
			}
		}
	}
}