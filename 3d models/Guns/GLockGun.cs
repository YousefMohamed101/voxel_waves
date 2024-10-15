using Godot;
using System;

public partial class GLockGun : Weapon
{
	[Export] public PackedScene BulletScene;
	[Export] public float BulletSpeed { get; set; } = 60.0f;
	[Export] public int MagazineSize { get; set; } = 17;
	[Export] public int clips { get; set; } = 5;
	[Export] public float FireRate { get; set; } = 0.5f; // Faster fire rate for automatic
	private Marker3D FiringPoint;
	private Timer Rateoffire;
	private bool mouse_left_down;
	private bool Walking;
	private Timer Reloadtime;
	private AudioStreamPlayer3D Gunshot;
	private AnimationPlayer Anime;
	private string currentAnimation = "IDLE";


	[Export] private OmniLight3D MBang;
	[Export] private GpuParticles3D MFlashP;

	[Export] private TextureRect crosshair;

	private RayCast3D _rayCast;
	private Camera3D _camera;
	private bool shot = false;
	public override void _Ready()
	{
		FiringPoint = GetNode<Marker3D>("Cube_008/FiringPoint");
		Rateoffire = GetNode<Timer>("Cube_008/RateOfFire");
		Rateoffire.WaitTime = FireRate;
		Gunshot = GetNode<AudioStreamPlayer3D>("Cube_008/Gunshot_Sound");
		Anime = GetNode<AnimationPlayer>("Cube_008/AnimationPlayer");
		Reloadtime = GetNode<Timer>("Cube_008/ReloadTime");
		Reloadtime.Stop();
		EmitMagazineChange(clips, MagazineSize);
		Walking = false;

		_rayCast = GetNode<RayCast3D>("/root/World/Player/Camera3D/RayCast3D");
		_camera = GetNode<Camera3D>("/root/World/Player/Camera3D");


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
	public void refill()
	{
		clips = 10;
		EmitMagazineChange(clips, MagazineSize);
	}
	public override void _PhysicsProcess(double delta)
	{
		Walking = Input.IsActionPressed("Forward") || Input.IsActionPressed("Backward") || Input.IsActionPressed("Right") || Input.IsActionPressed("Left");

		if (mouse_left_down && Rateoffire.IsStopped() && Reloadtime.IsStopped() && MagazineSize != 0)
		{
			_rayCast.ForceRaycastUpdate(); // Update raycast to get the latest hit information

			Vector3 targetPoint;
			Vector3 bulletDirection = -_camera.GlobalBasis.Z;

			// Check if the ray hit something
			if (_rayCast.IsColliding())
			{
				targetPoint = _rayCast.GetCollisionPoint();
				bulletDirection = (targetPoint - FiringPoint.GlobalPosition).Normalized();
			}



			// Shoot the bullet
			OnPlayerShoot(BulletScene, FiringPoint.GlobalBasis, bulletDirection, FiringPoint.GlobalPosition, BulletSpeed);
			Gunshot.Play();
			PlayAnimation("shooting");
			MagazineSize -= 1;
			EmitMagazineChange(clips, MagazineSize);
			if (MagazineSize == 0 || !Rateoffire.IsStopped())
			{
				mouse_left_down = false;

			}
			MBang.Visible = false;
			Rateoffire.Start();
		}
		else if (Input.IsActionPressed("Reloading"))
		{
			MFlashP.Emitting = false;
			MBang.Visible = false;


			if (Reloadtime.IsStopped() && clips != 0)
			{
				clips -= 1;
				MagazineSize = 17;
				EmitMagazineChange(clips, MagazineSize);
			}

			Reloadtime.Start();
			PlayAnimation("Reload");
		}
		else if (!mouse_left_down && Reloadtime.IsStopped())
		{
			if (Walking)
			{
				PlayAnimation("Moving");

			}
			else
			{
				PlayAnimation("IDLE");
			}
			MFlashP.Emitting = false;
			MBang.Visible = false;



		}
	}


	private void PlayAnimation(string animationName)
	{
		if (currentAnimation == animationName) return; // Prevent re-triggering the same animation
		Anime.Play(animationName);
		currentAnimation = animationName;
	}

	private void OnPlayerShoot(PackedScene bulletScene, Basis direction, Vector3 DirectionB, Vector3 location, float BulletSpeedE)
	{
		var spawnedBullet = bulletScene.Instantiate<RigidBody3D>();
		spawnedBullet.Position = location;
		spawnedBullet.Basis = direction;
		spawnedBullet.LinearVelocity = DirectionB * BulletSpeedE;

		MFlashP.Emitting = true;
		MBang.Visible = true;
		GetTree().Root.AddChild(spawnedBullet);

	}

}
