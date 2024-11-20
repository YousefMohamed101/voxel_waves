using Godot;
using System;

public partial class RigidE : RigidBody3D
{
	private Skeleton3D skele;
	private Timer Delete;
	public override void _Ready()
	{
		skele = GetNode<Skeleton3D>("Armature/Skeleton3D");
		skele.PhysicalBonesStartSimulation();
		Delete = GetNode<Timer>("Timer");
		Delete.Timeout += () => QueueFree();


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
