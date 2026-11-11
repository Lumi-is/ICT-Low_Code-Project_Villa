using Godot;
using System;

interface IClickable
{
	Sprite2D GetGraphics();
	void Highlight(); 
	void Reset();
	void OnClickEvent();
}
