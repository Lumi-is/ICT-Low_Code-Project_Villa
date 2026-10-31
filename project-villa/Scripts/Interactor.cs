using Godot;
using System;

public partial class Interactor : Node2D
{
    // Which *clickable* object is mouse currently hovering over?
    IClickable clickable;
    Vector2 mousePos;

    public override void _PhysicsProcess(double delta)
    {
        var space = GetViewport().World2D.DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(mousePos, Vector2.Zero);
        var result = space.IntersectRay(query);

        if (result.Count > 0)
        {
            GD.Print("Hit at point: ", result["position"]);
        }
    }

    //Get whether or not the object the cursor is hovering over is of interface IClickable??
    public override void _Process(double delta)
    {
        // clickable = ???
        mousePos = GetGlobalMousePosition();

        if(Input.IsActionJustPressed("Mouse1"))
        {
            clickable.OnClickEvent();
        }
    }
}
