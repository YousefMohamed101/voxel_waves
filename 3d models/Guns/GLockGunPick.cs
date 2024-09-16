using Godot;
using System;

public partial class GLockGunPick : Weapon
{
	private Timer despawn;
	private AnimationPlayer Anim;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		despawn = GetNode<Timer>("Timer");
		despawn.Timeout += QueueFree;
		Anim = GetNode<AnimationPlayer>("AnimationPlayer");
		Anim.Play("Hover");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
