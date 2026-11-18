using Godot;
using Grid;
using System;

public partial class ExampleInteractable : Node2D, IClickable, IDraggable
{
    [Export] Sprite2D graphics;
    public Sprite2D GetGraphics()
    {
        // Return graphics set from inspector
        return graphics;
    }

    #region IClickable

    public void Highlight()
    {
        // What happens when hovering over object
    }

    public void OnClickEvent()
    {
        // What happens when object is clicked
    }

    public void Reset()
    {
        // Return object to original state
    }

    #endregion

    #region IDraggable

    // Getgraphics is a common method between IClickable and IDraggable

    public void Drag(Vector2 mousePos)
    {
        // What should happen if object is being dragged, for most purposes should be:
        Interactor.SnapToMouse(this, mousePos);
        // But can also have specific implementation for objects which differ from normal interactables.
    }

    public void Release()
    {
        // What happens when object is released
    }


    #endregion




}
