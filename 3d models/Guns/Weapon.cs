using Godot;
using System;

public partial class Weapon : Node3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public string WeaponName { get; set; } = "Default Weapon";
	[Export] public int Damage { get; set; } = 10;


	[Signal]
	public delegate void PlayerShootEventHandler(PackedScene bullet, Basis direction, Vector3 directionB, Vector3 location, float bulletSpeedE);


	public virtual void OnBodyEntered()
	{

	}

}
