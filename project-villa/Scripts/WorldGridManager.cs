using Godot;
using System;
using System.Calculations;

namespace Grid
{
	static class WorldGridManager
	{
		public static void SnapToGrid(Node2D someSnappable, Vector2 position)
		{
			Node2D snappable = someSnappable.GetOwner<Node2D>();

            var jumpInterval = 100f;
            var xPosition = position.X;
			var yPosition = position.Y;
			float xOffset = 0;
			float yOffset = 0;
			float xJumpCount = 0;
			float yJumpCount = 0;

			Vector2 negativeSnapPos = new();
			Vector2 positiveSnapPos = new();
			Vector2 finalSnapPos = new();

            xOffset = xPosition % jumpInterval; 
            xJumpCount = (xPosition - xOffset) / jumpInterval;

            negativeSnapPos.X = xJumpCount * jumpInterval + jumpInterval/2;
			positiveSnapPos.X = (xJumpCount + 1) * jumpInterval + jumpInterval/2;

			yOffset = yPosition % jumpInterval;
			yJumpCount = (yPosition - yOffset) / jumpInterval;

			negativeSnapPos.Y = yJumpCount * jumpInterval + jumpInterval/2;
			positiveSnapPos.Y = (yJumpCount + 1) * jumpInterval + jumpInterval/2;

			if(xPosition % jumpInterval / (jumpInterval/2) == 1) return;

			GD.Print(xJumpCount * jumpInterval + jumpInterval / 2);
			GD.Print("X Position: "+xPosition);
			
			if (xPosition > xJumpCount * jumpInterval + jumpInterval / 2)
			{
				finalSnapPos.X = negativeSnapPos.X;
			}
			else
			{
				finalSnapPos.X = positiveSnapPos.X;
			}

			if(yPosition % jumpInterval / (jumpInterval/2) == 1) return;

			GD.Print(yJumpCount * jumpInterval + jumpInterval / 2);
			GD.Print("Y Position: "+yPosition);
			if (yPosition > yJumpCount * jumpInterval + jumpInterval / 2)
			{
				finalSnapPos.Y = negativeSnapPos.Y;
			}
			else
			{
				finalSnapPos.Y = positiveSnapPos.Y;
			}

			snappable.Position = finalSnapPos;

			//GD.Print(snappable.Position);
		}
	}
}