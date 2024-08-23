using Godot;
using System;

public partial class Weapon : Node3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public string WeaponName { get; set; } = "Default Weapon";
	[Export] public int Damage { get; set; } = 10;
	[Export] public float FireRate { get; set; } = 0.1f;
	[Signal]
	public delegate void PlayerShootEventHandler(PackedScene bullet, Basis direction, Vector3 directionB, Vector3 location, float bulletSpeedE);



	public virtual void UseWeapon()
	{
		GD.Print("Using weapon: " + WeaponName);
		// This method should be overridden by specific weapon types
	}
}
