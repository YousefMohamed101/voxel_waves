using Godot;
using System;

public partial class Ak47 : Node3D
{
	[Signal]
	public delegate void ShootEventHandler(PackedScene bullet, Vector3 direction, Vector3 location, float BulletSpeedE);
	[Export]
	public PackedScene BulletScene = GD.Load<PackedScene>("res://3d models/BulletRigid.tscn");   // Reference to the bullet scene
	[Export]
	public float FireRate { get; set; } = 0.2f;  // Time between shots
	[Export]
	public float BulletSpeed { get; set; } = 700.0f;  // Speed of the bullet
	private Marker3D firingPoint;
	private Camera3D camerainfo;

	public override void _Ready()
	{
		firingPoint = GetNode<Marker3D>("FiringPoint");
		camerainfo = GetNode<Camera3D>("/root/TestLevel/Player/Camera3D");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
			{
				EmitSignal(SignalName.Shoot, BulletScene, firingPoint.GlobalBasis, -camerainfo.GlobalBasis.Z, firingPoint.GlobalPosition, BulletSpeed);
			}
		}
	}

	public override void _Process(double delta)
	{

	}

	private void ShootBullet()
	{
		if (BulletScene != null)
		{
			// Instance the bullet scene
			RigidBody3D bullet = (RigidBody3D)BulletScene.Instantiate();
			// Set the bullet's position and rotation to the firing point
			bullet.Position = firingPoint.Position;
			bullet.Rotation = firingPoint.Rotation;
			Vector3 direction = firingPoint.GlobalBasis.X;
			bullet.LinearVelocity = direction * BulletSpeed;

			GetParent().AddChild(bullet);


		}
	}
}