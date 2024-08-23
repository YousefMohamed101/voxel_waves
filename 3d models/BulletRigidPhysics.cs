using Godot;
using System;

public partial class BulletRigidPhysics : RigidBody3D
{

	[Export]
	private float LifeTime = 2.0f;
	private Timer Lifetime;
	public override void _Ready()
	{// Example radius of the bullet in meters
		Lifetime = GetNode<Timer>("Timer");
		Lifetime.WaitTime = LifeTime;
		Lifetime.Start();
		Lifetime.Timeout += () => QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

	}
}
