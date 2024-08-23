using Godot;
using System;

public partial class Glockblack : Node3D
{
	// Called when the node enters the scene tree for the first time.
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
		camerainfo = GetNode<Camera3D>("/root/World/Player/Camera3D");
		RateOfFire = GetNode<Timer>("RateOfFire");

		RateOfFire.WaitTime = FireRate;
		RateOfFire.OneShot = true;




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


	}

	private void TShoot()
	{
		// Emit the shoot signal
		EmitSignal(SignalName.Shoot, BulletScene, firingPoint.GlobalBasis, -camerainfo.GlobalBasis.Z, firingPoint.GlobalPosition, BulletSpeed);
		RateOfFire.Start();
	}

}
