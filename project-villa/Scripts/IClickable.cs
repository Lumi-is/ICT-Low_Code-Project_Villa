using Godot;
using System;

interface IClickable
{
	void Highlight(); 
	void Reset();
	void OnClickEvent();
}
