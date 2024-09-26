using Godot;
using System;

public partial class Zombie1Rigid : Node3D
{
	// Called when the node enters the scene tree for the first time.
	private Timer Delete;
	public override void _Ready()
	{
		Delete = GetNode<Timer>("Timer");

		Delete.Timeout += unspawn;
		Delete.Start();
	}
	private void unspawn()
	{
		GD.Print("Deleted");
		QueueFree();
	}

	public override void _Process(double delta)
	{
	}
}
