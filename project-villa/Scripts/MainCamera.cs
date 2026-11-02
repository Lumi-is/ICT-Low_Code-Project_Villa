using Godot;
using System;

public partial class MainCamera : Node2D
{

	[Export]
	public float speed = 10.0f;

	Vector2 mousePos = Vector2.Zero; 
	Vector2 lastMousePos = Vector2.Zero;
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		lastMousePos = mousePos;
		mousePos = GetGlobalMousePosition();
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("Mouse3"))
		{
			var deltamouse = mousePos-lastMousePos;
			deltamouse *= -1;
			velocity += deltamouse;

		
		}
		else
		{
			if (Input.IsActionPressed("Right"))
			{
				velocity.X += speed;
			}

			if (Input.IsActionPressed("Left"))
			{
				velocity.X -= speed;
			}

			if (Input.IsActionPressed("Down"))
			{
				velocity.Y += speed;
			}
			if (Input.IsActionPressed("Up"))
			{
				velocity.Y -= speed;
			}
		}


		

		Position += velocity;

	}
}