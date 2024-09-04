using Godot;
using System;

public partial class Glockblack : Weapon
{
	// Called when the node enters the scene tree for the first time.
	[Export] public PackedScene BulletScene;
	[Export] public float BulletSpeed { get; set; } = 60.0f;
	[Export] public int MagazineSize { get; set; } = 30;
	[Export] public float FireRate { get; set; } = 0.05f; // Faster fire rate for automatic
	private Marker3D FiringPoint;
	private Timer Rateoffire;
	private bool mouse_left_down;
	public override void _Ready()
	{
		FiringPoint = GetNode<Marker3D>("FiringPoint");
		Rateoffire = GetNode<Timer>("RateOfFire");
		Rateoffire.WaitTime = FireRate;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
			{
				mouse_left_down = true;
			}
			else
			{
				mouse_left_down = false;
			}
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		if (mouse_left_down && Rateoffire.IsStopped())
		{
			OnPlayerShoot(BulletScene, FiringPoint.GlobalBasis, FiringPoint.GlobalBasis.X, FiringPoint.GlobalPosition, BulletSpeed);
			Rateoffire.Start();
		}

	}
	private void OnPlayerShoot(PackedScene bulletScene, Basis direction, Vector3 DirectionB, Vector3 location, float BulletSpeedE)
	{
		var spawnedBullet = bulletScene.Instantiate<RigidBody3D>();
		spawnedBullet.Position = location;
		spawnedBullet.Basis = direction;
		spawnedBullet.LinearVelocity = DirectionB * BulletSpeedE;
		GetTree().Root.AddChild(spawnedBullet);
	}



}
