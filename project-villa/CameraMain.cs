using Godot;
using System;

public partial class CameraMain : Node2D
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

		if (Input.IsActionPressed("MouseCamera"))
		{
			var deltamouse = mousePos-lastMousePos;
			deltamouse *= -1;
			velocity += deltamouse;

		
		}
		else
		{
			if (Input.IsActionPressed("CameraRight"))
			{
				velocity.X += speed;
			}

			if (Input.IsActionPressed("CameraLeft"))
			{
				velocity.X -= speed;
			}

			if (Input.IsActionPressed("CameraDown"))
			{
				velocity.Y += speed;
			}
			if (Input.IsActionPressed("CameraUp"))
			{
				velocity.Y -= speed;
			}
		}


		

		Position += velocity;

	}
}