using Godot;
using System;

interface IClickable
{
	void Highlight(); 
	void StopHighlight();
	void OnClickEvent();
	void OnMouseEntered();
	void OnMouseExited();
}
