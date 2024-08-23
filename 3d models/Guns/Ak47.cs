using Godot;
using System;

public partial class Ak47 : Weapon
{
	[Export] public PackedScene BulletScene;
	[Export] public float BulletSpeed { get; set; } = 60.0f;
	[Export] public int MagazineSize { get; set; } = 30;
	[Export] public new float FireRate { get; set; } = 0.1f; // Faster fire rate for automatic
	private Marker3D FiringPoint;
	public override void _Ready()
	{
		FiringPoint = GetNode<Marker3D>("FiringPoint");
	}
	public override void UseWeapon()
	{

		EmitSignal(SignalName.PlayerShoot, BulletScene, FiringPoint.GlobalBasis, FiringPoint.GlobalBasis.X, FiringPoint.GlobalPosition, BulletSpeed);
	}

}




