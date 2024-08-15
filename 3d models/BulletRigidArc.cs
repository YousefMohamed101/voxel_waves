using Godot;
using System;

public partial class BulletRigidArc : Area3D
{

	[Export]
	public float Speed = 5.0f;
	private Vector3 velocity;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += velocity * (float)delta;
	}
}
