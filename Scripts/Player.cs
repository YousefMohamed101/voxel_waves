using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public float JumpVelocity;
	public float W_Gravity = 9.8f;

	[Export] private PackedScene StartingWeaponScene;
	private Camera3D camera;
	[Export] public float Sensitivity = 0.1f;
	[Export] public float MaxPitch = 90.0f;
	[Export] public float MinPitch = -90.0f;
	[Export] public float Strength = 8.0f;

	private Marker3D Equip;
	private float _yaw = 0.0f;
	private float _pitch = 0.0f;
	[Export] private float Run_Speed = 1.5f;
	[Export] private float Weight = 1.5f;
	private Weapon equippedWeapon;
	private bool mouse_left_down = false;
	private Timer OnFireRate;
	public override void _Ready()
	{
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
		camera = GetNode<Camera3D>("Camera3D");
		camera.Rotation = new Vector3(0, 0, 0);
		OnFireRate = GetNode<Timer>("Firerate");
		equippedWeapon = null;
		if (StartingWeaponScene != null)
		{
			Weapon startingWeapon = StartingWeaponScene.Instantiate<Weapon>();
			EquipWeapon(startingWeapon);
		}
	}

	private void OnPlayerShoot(PackedScene bullet, Basis direction, Vector3 DirectionB, Vector3 location, float BulletSpeedE)
	{
		var spawnedBullet = bullet.Instantiate<RigidBody3D>();
		spawnedBullet.Position = location;
		spawnedBullet.Basis = direction;
		spawnedBullet.LinearVelocity = DirectionB * BulletSpeedE;
		GetTree().Root.AddChild(spawnedBullet);

	}

	public void EquipWeapon(Weapon weapon)
	{
		if (equippedWeapon != null)
		{
			equippedWeapon.PlayerShoot -= OnPlayerShoot;
			equippedWeapon.QueueFree(); // Remove the old weapon
		}

		equippedWeapon = weapon;
		GetNode<Marker3D>("Camera3D/Marker3D").AddChild(weapon);
		equippedWeapon.PlayerShoot += OnPlayerShoot;
		OnFireRate.WaitTime = equippedWeapon.FireRate;
	}




	public override void _PhysicsProcess(double delta)
	{
		if (mouse_left_down && OnFireRate.IsStopped() && equippedWeapon != null)
		{
			equippedWeapon.UseWeapon();
			OnFireRate.Start();
		}

		Vector3 velocity = Velocity;
		JumpVelocity = Mathf.Sqrt(2 * Strength * W_Gravity);
		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta * Weight;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity / Weight;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");
		Vector3 forward = camera.GlobalTransform.Basis.Z.Normalized();
		Vector3 right = camera.GlobalTransform.Basis.X.Normalized();
		Vector3 direction = (forward * inputDir.Y + right * inputDir.X).Normalized();
		bool isRunning = Input.IsActionPressed("Run");
		if (direction != Vector3.Zero)
		{
			float currentSpeed = Speed;

			if (isRunning)
			{
				currentSpeed *= Run_Speed;
			}
			//GD.Print(currentSpeed);

			velocity.X = direction.X * currentSpeed;
			velocity.Z = direction.Z * currentSpeed;

		}
		else if (direction == Vector3.Zero && IsOnFloor())
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion R)
		{
			_yaw -= R.Relative.X * Sensitivity;
			_pitch -= R.Relative.Y * Sensitivity;

			// Clamp the pitch to prevent flipping
			_pitch = Mathf.Clamp(_pitch, MinPitch, MaxPitch);

			// Apply the rotation to the camera
			camera.RotationDegrees = new Vector3(_pitch, _yaw, 0);
		}
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

	public override void _UnhandledInput(InputEvent @event)
	{

		if (@event is InputEventKey e && e.IsPressed() && e.Keycode == Key.Escape)
		{
			if (DisplayServer.MouseGetMode() == DisplayServer.MouseMode.Captured)
			{
				DisplayServer.MouseSetMode(DisplayServer.MouseMode.Visible);
			}
			else
			{
				DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
			}
		}
	}
}

