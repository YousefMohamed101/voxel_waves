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

	[Export] private float MFlash = 0.1f;
	[Export] private OmniLight3D MBang;
	[Export] private GpuParticles3D MFlashP;
	public override void _Ready()
	{
		FiringPoint = GetNode<Marker3D>("FiringPoint");
		Rateoffire = GetNode<Timer>("RateOfFire");
		Rateoffire.WaitTime = FireRate;
		Gunshot = GetNode<AudioStreamPlayer3D>("Gunshot_Sound");
		Anime = GetNode<AnimationPlayer>("AnimationPlayer");
		Reloadtime = GetNode<Timer>("ReloadTime");
		Reloadtime.Stop();
		EmitMagazineChange(clips, MagazineSize);
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
		Walking = Input.IsActionPressed("Forward") || Input.IsActionPressed("Backward") || Input.IsActionPressed("Right") || Input.IsActionPressed("Left");

		if (mouse_left_down && Rateoffire.IsStopped() && Reloadtime.IsStopped() && MagazineSize != 0)
		{
			OnPlayerShoot(BulletScene, FiringPoint.GlobalBasis, FiringPoint.GlobalBasis.X, FiringPoint.GlobalPosition, BulletSpeed);

			Gunshot.Play();
			PlayAnimation("shooting");
			MagazineSize -= 1;
			EmitMagazineChange(clips, MagazineSize);

			if (MagazineSize == 0 || !Rateoffire.IsStopped())
			{
				mouse_left_down = false;
			}
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
