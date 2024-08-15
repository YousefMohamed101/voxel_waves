using Godot;
using System;

public partial class TestLevel : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void OnPlayerShoot(PackedScene bullet, Basis direction, Vector3 DirectionB, Vector3 location, float BulletSpeedE)
	{
		var spawnedBullet = bullet.Instantiate<RigidBody3D>();
		AddChild(spawnedBullet);
		spawnedBullet.Position = location;
		spawnedBullet.Basis = direction;
		spawnedBullet.LinearVelocity = DirectionB * BulletSpeedE;

	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
