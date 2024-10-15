using Godot;
using System;

public partial class Unpause2 : CanvasLayer
{
    private CenterContainer InGameMenu;
    private ColorRect MenuShade;
    public override void _Ready()
    {
        InGameMenu = GetNode<CenterContainer>("CenterContainer");
        MenuShade = GetNode<ColorRect>("ColorRect");
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey e && e.IsPressed() && e.Keycode == Key.Escape)
        {
            GetViewport().SetInputAsHandled();
            if (GetTree().Paused)
            {
                GetTree().Paused = false;
                InGameMenu.Visible = false;
                MenuShade.Visible = false;
                DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
            }
        }
    }
}
