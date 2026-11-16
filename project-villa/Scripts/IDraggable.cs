using Godot;
using System;

interface IDraggable
{
	Sprite2D GetGraphics();
	void Drag(Vector2 mousePos);

	void Release();

}