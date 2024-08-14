using Godot;
using System;

public partial class TestLevel : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void OnPlayerShoot(PackedScene bullet, Vector3 direction, Vector3 location, float BulletSpeedE)
	{
		var spawnedBullet = bullet.Instantiate<RigidBody3D>();
		AddChild(spawnedBullet);
		spawnedBullet.Position = location;
		spawnedBullet.Rotation = direction;
		spawnedBullet.LinearVelocity = direction * BulletSpeedE;
		GD.Print("Yousef Emitted");

	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
