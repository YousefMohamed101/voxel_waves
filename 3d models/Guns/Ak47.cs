using Godot;
using System;

public partial class Ak47 : Node3D
{
	[Signal]
	public delegate void ShootEventHandler(PackedScene bullet, Vector3 direction, Vector3 location, float BulletSpeedE);
	[Export]
	public PackedScene BulletScene = GD.Load<PackedScene>("res://3d models/BulletRigid.tscn");   // Reference to the bullet scene
	[Export]
	public float FireRate { get; set; } = 2.0f;  // Time between shots
	[Export]
	public float BulletSpeed { get; set; } = 700.0f;  // Speed of the bullet
	private Marker3D firingPoint;
	private Camera3D camerainfo;
	private bool mouse_left_down = false;
	private Timer RateOfFire;
	public override void _Ready()
	{
		firingPoint = GetNode<Marker3D>("FiringPoint");
		camerainfo = GetNode<Camera3D>("/root/TestLevel/Player/Camera3D");
		RateOfFire = GetNode<Timer>("RateOfFire");

		RateOfFire.WaitTime = FireRate;
		RateOfFire.OneShot = true;
		RateOfFire.Timeout += () => RateOfShoot();



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

	public override void _Process(double delta)
	{
		if (mouse_left_down)
		{
			if (RateOfFire.IsStopped())
			{
				RateOfFire.Start(); // Start the Timer if it's not already running
			}

		}
	}

	private void RateOfShoot()
	{
		EmitSignal(SignalName.Shoot, BulletScene, firingPoint.GlobalBasis, -camerainfo.GlobalBasis.Z, firingPoint.GlobalPosition, BulletSpeed);
	}
}




