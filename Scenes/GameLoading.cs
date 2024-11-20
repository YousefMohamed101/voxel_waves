using Godot;
using System;

public partial class GameLoading : ColorRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("Timer").Timeout += () => GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("World.tscn");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
