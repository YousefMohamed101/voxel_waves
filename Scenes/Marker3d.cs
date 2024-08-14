using Godot;
using System;

public partial class Marker3d : Marker3D
{
	public PackedScene GunScene = GD.Load<PackedScene>("res://3d models/Guns/ak_47.tscn");
	public Ak47 GUNEquip;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get the reference to the Camera3D node
		//GUNEquip = (Ak47)GunScene.Instantiate(); ;

		//CallDeferred(nameof(AddChildDeferred), GUNEquip);
	}
	private void AddChildDeferred(Node child)
	{
		AddChild(child);
		GD.Print("Spawned");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
