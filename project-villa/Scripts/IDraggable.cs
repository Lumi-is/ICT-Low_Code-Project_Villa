using Godot;
using System;

interface IDraggable
{
	void Drag(Vector2 mousePos);

	void Release();

}