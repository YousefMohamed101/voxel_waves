using Godot;
using System;

public partial class MouseTexture : Node
{
	public Texture2D tab_cursor;
	public override void _Ready()
	{
		tab_cursor = ResourceLoader.Load<Texture2D>("res://UI/Mouse/cursor_select_tap.png");
		Input.SetCustomMouseCursor(ResourceLoader.Load<Texture2D>("res://UI/Mouse/cursor_default.png"));
		Input.SetCustomMouseCursor(ResourceLoader.Load<Texture2D>("res://UI/Mouse/cursor_grabbing.png"), Input.CursorShape.Drag);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton click && click.ButtonIndex == MouseButton.Left)
		{
			if (click.Pressed)
			{
				Input.SetCustomMouseCursor(tab_cursor);
			}
			else
			{
				Input.SetCustomMouseCursor(ResourceLoader.Load<Texture2D>("res://UI/Mouse/cursor_default.png"));
			}
		}
	}


}
