using Godot;
using System;

public partial class RigidE : Node3D
{
	private Skeleton3D skele;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		skele = GetNode<Skeleton3D>("Armature/Skeleton3D");
		skele.PhysicalBonesStartSimulation();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
