using Godot;
using System;

public partial class Unpause : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey e && e.IsPressed() && e.Keycode == Key.Escape)
		{
			GetViewport().SetInputAsHandled();
			if (GetTree().Paused)
			{
				GetTree().Paused = false;
				DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
			}
		}
	}
}
