using Godot;
using System;

public partial class GLockGun : Weapon
{
	[Export] public PackedScene BulletScene;
	[Export] public float BulletSpeed { get; set; } = 60.0f;
	[Export] public int MagazineSize { get; set; } = 30;
	[Export] public float FireRate { get; set; } = 0.5f; // Faster fire rate for automatic
	private Marker3D FiringPoint;
	private Timer Rateoffire;
	private bool mouse_left_down;
	private bool Walking;
	private Timer Reloadtime;
	private AudioStreamPlayer3D Gunshot;
	private AnimationPlayer Anime;
	public override void _Ready()
	{
		FiringPoint = GetNode<Marker3D>("FiringPoint");
		Rateoffire = GetNode<Timer>("RateOfFire");
		Rateoffire.WaitTime = FireRate;
		Gunshot = GetNode<AudioStreamPlayer3D>("Gunshot_Sound");
		Anime = GetNode<AnimationPlayer>("AnimationPlayer");
		Reloadtime = GetNode<Timer>("ReloadTime");
		Reloadtime.Stop();
		Walking = false;

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
		if (Input.IsActionPressed("Forward") || Input.IsActionPressed("Backward") || Input.IsActionPressed("Right") || Input.IsActionPressed("Left"))
		{

			Walking = true;


		}
		else
		{
			Walking = false;
		}
		if (mouse_left_down && Rateoffire.IsStopped() && Reloadtime.IsStopped())
		{
			OnPlayerShoot(BulletScene, FiringPoint.GlobalBasis, FiringPoint.GlobalBasis.X, FiringPoint.GlobalPosition, BulletSpeed);
			Gunshot.Play();
			Anime.Play("shooting");
			Rateoffire.Start();
		}
		else if (Input.IsActionPressed("Reloading"))
		{
			Reloadtime.Start();
			Anime.Play("Reload");
		}
		else if (mouse_left_down == false && Reloadtime.IsStopped())
		{



			Anime.Play("IDLE");


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
