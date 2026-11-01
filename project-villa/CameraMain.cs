using Godot;
using System;

public partial class CameraMain : Node2D
{

	[Export]
	public float speed = 10.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;

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

		Position += velocity;

	}
}