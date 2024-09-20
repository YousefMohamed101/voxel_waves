using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public float JumpVelocity;
	public float W_Gravity = 9.8f;

	// Correctly point to the .tscn files for your weapons
	private PackedScene Ak47 = GD.Load<PackedScene>("res://3d models/Guns/ak_47.tscn");
	private PackedScene Glock = GD.Load<PackedScene>("res://3d models/Guns/GLockGun.tscn");
	private Camera3D camera;
	[Export] public float Sensitivity = 0.1f;
	[Export] public float MaxPitch = 90.0f;
	[Export] public float MinPitch = -90.0f;
	[Export] public float Strength = 8.0f;
	public int magazine = 0;

	private Marker3D Equip;
	private float _yaw = 0.0f;
	private float _pitch = 0.0f;
	[Export] private float Run_Speed = 1.5f;
	[Export] private float Weight = 1.5f;
	private Weapon equippedWeapon;
	private bool mouse_left_down = false;

	public Marker3D marker;
	Label hud;
	private bool isWeaponEquipped = false;


	public override void _Ready()
	{
		hud = GetNode<Label>("Control/BoxContainer/RichTextLabel");
		marker = GetNode<Marker3D>("Camera3D/Marker3D");
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
		camera = GetNode<Camera3D>("Camera3D");


		equippedWeapon = null;

		// Instantiate the starting weapon correctly
		Node3D startingWeapon = Ak47.Instantiate() as Node3D;
		if (startingWeapon != null && startingWeapon is Weapon newWeapon)
		{
			magazine = 10; // Set the magazine size from the weapon
			EquipWeapon(newWeapon); // Equip the weapon with the correct magazine size

		}

		Area3D playerBox = GetNode<Area3D>("Playerbox");
		playerBox.BodyEntered += _on_playerbox_body_entered;
	}

	public void _on_playerbox_body_entered(Node3D body)
	{

		if (body is Weapon)
		{
			// Instantiate and equip weapon based on the name
			Node3D weaponInstance = null;

			if (body is GLockGunPick)
			{
				weaponInstance = Glock.Instantiate() as Node3D;
			}
			else if (body is Ak47Pick)
			{
				weaponInstance = Ak47.Instantiate() as Node3D;
			}

			// Equip the weapon if instantiated successfully
			if (weaponInstance != null && weaponInstance is Weapon newWeapon)
			{
				EquipWeapon(newWeapon); // Equip the weapon with the correct magazine size
				body.QueueFree(); // Remove the old weapon
			}
		}
		else if (body is Ammobox)
		{
			var equippedlife = marker.GetChild(0);


			if (equippedlife.HasMethod("refill"))
			{
				GD.Print(body.Name);
				equippedlife.Call("refill");
			}

		}
	}



	public void EquipWeapon(Weapon weapon)
	{
		// Remove the currently equipped weapon, if any
		if (marker.GetChildCount() > 0)
		{
			marker.GetChild(0).QueueFree();
			weapon.MagazineChange -= ONMagazineChange; // Free the old weapon from memory
		}
		weapon.MagazineChange += ONMagazineChange;



		// Add the new weapon to the marker
		marker.AddChild(weapon);
		equippedWeapon = weapon; // Update the equipped weapon reference


	}

	private void ONMagazineChange(int ClipCount, int magazineCount)
	{

		hud.Text = $"{magazineCount}/{ClipCount}";


	}



	public override void _PhysicsProcess(double delta)
	{


		Vector3 velocity = Velocity;
		JumpVelocity = Mathf.Sqrt(2 * Strength * W_Gravity);

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta * Weight;
		}

		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity / Weight;
		}

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

			_pitch = Mathf.Clamp(_pitch, MinPitch, MaxPitch);

			camera.RotationDegrees = new Vector3(_pitch, _yaw, 0);
		}


	}


}
