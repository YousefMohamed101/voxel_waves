using Godot;
using System;

public partial class Skeleton3d : Skeleton3D
{

	public override void _Ready()
	{
		PhysicalBonesStartSimulation();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
