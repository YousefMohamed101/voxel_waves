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
	private PackedScene Bomb = GD.Load<PackedScene>("res://BombEquiped.tscn");
	private Weapon[] weaponInstances = new Weapon[2];
	private Camera3D camera;
	[Export] public float Sensitivity = 0.1f;
	[Export] public float MaxPitch = 90.0f;
	[Export] public float MinPitch = -90.0f;
	[Export] public float Strength = 8.0f;
	public int magazine = 0;
	private int currentWeaponIndex = 0;
	private Marker3D Equip;
	private float _yaw = 0.0f;
	private float _pitch = 0.0f;
	[Export] private float Run_Speed = 1.5f;
	[Export] private float Weight = 1.5f;
	private Weapon equippedWeapon;
	private bool mouse_left_down = false;

	public Marker3D marker;
	[Export]
	public RayCast3D Raycast;

	Label hud;
	private bool isWeaponEquipped = false;
	private int health;
	private ProgressBar Healthbar;
	private OptionData _data;
	private GameDAta _data2;
	private bool Dead = false;
	private bool deadR = false;
	private bool ADead = false;
	public AnimationPlayer anime;
	[Export] CanvasLayer canva;
	public GameMusicHandler DJ;

	public override void _Ready()
	{
		if (Raycast == null)
		{
			GD.PrintErr("RayCast3D node not assigned!");
			return;
		}


		health = 20;
		hud = GetNode<Label>("Control/BoxContainer/RichTextLabel");
		marker = GetNode<Marker3D>("Camera3D/Marker3D");
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
		camera = GetNode<Camera3D>("Camera3D");
		Healthbar = GetNode<ProgressBar>("Control/Health/ProgressBar");
		Area3D playerBox = GetNode<Area3D>("Playerbox");
		equippedWeapon = null;
		Healthbar.MaxValue = health;
		anime = GetNode<AnimationPlayer>("AnimationPlayer");
		canva = GetNode<CanvasLayer>("/root/World/GameOver");
		DJ = GetNode<GameMusicHandler>("/root/GameMusicHandler");

		AddToInventory(Glock, 0);
		AddToInventory(Bomb, 1);

		EquipWeapon(0);



		playerBox.BodyEntered += _on_playerbox_body_entered;
	}
	private void _LoadData(OptionData data)
	{
		_data = data;
		camera.Fov = _data.FovSlide;
		Sensitivity = _data.Sensitivity;


	}
	private void _LoadData2(GameDAta data)
	{
		_data2 = data;
		deadR = _data2.deads;
	}
	private void RuntimeLoad()
	{
		string fileName = "res://Resources/Options.tres";
		string fileName2 = "res://Resources/GameData.tres";
		if (ResourceLoader.Exists(fileName))
		{

			_LoadData(ResourceLoader.Load<OptionData>(fileName, null, ResourceLoader.CacheMode.Ignore));
			_LoadData2(ResourceLoader.Load<GameDAta>(fileName2, null, ResourceLoader.CacheMode.Ignore));
		}

	}

	private void SaveData()
	{
		string fileName = "res://Resources/Options.tres";
		ResourceSaver.Save(_data, fileName);
		string fileName2 = "res://Resources/GameData.tres";
		ResourceSaver.Save(_data2, fileName2);

	}


	public void _on_playerbox_body_entered(Node3D body)
	{
		GD.Print("body.Name");

		if (body is Weapon)
		{
			// Instantiate and equip weapon based on the name
			Node3D weaponInstance = null;

			if (body is GLockGunPick)
			{

				AddToInventory(Glock, 0);
				EquipWeapon(0);


			}
			else if (body is Ak47Pick)
			{

				AddToInventory(Ak47, 0);
				EquipWeapon(0);
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
		body.QueueFree();
	}

	private void recieve_damage(int x)
	{
		health -= x;
		Healthbar.Value = health;
		if (health <= 0 && ADead == false)
		{
			Dead = true;
			anime.Play("Death");
			ADead = true;
			DJ.deadss = true;
			RuntimeLoad();
			_data2.highscoreregister();
			SaveData();
			GetTree().CreateTimer(2.0).Timeout += deathscreen;


		}
	}
	public void deathscreen()
	{
		canva.Visible = true;
		GetParent().GetTree().Paused = true;
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Visible);
	}


	public void EquipWeapon(int weaponIndex)
	{
		if (weaponIndex < 0 || weaponIndex >= weaponInstances.Length || weaponInstances[weaponIndex] == null)
		{
			GD.PrintErr("Invalid weapon index or weapon not available!");
			return;
		}
		Weapon weaponToEquip = weaponInstances[weaponIndex];
		// Remove currently equipped weapon from marker if any
		if (marker.GetChildCount() > 0)
		{
			var currentWeapon = marker.GetChild(0) as Weapon;
			if (currentWeapon != null)
			{
				marker.RemoveChild(currentWeapon);

			}
		}

		// Equip the stored weapon instance

		marker.AddChild(weaponToEquip);
		currentWeaponIndex = weaponIndex;
		if (weaponToEquip is Ak47 g47)
		{
			weaponToEquip.MagazineChange += ONMagazineChange;
			weaponToEquip.EmitMagazineChange(g47.clips, g47.MagazineSize);
		}
		else if (weaponToEquip is GLockGun gl)
		{
			weaponToEquip.MagazineChange += ONMagazineChange;
			weaponToEquip.EmitMagazineChange(gl.clips, gl.MagazineSize);
		}
		else if (weaponToEquip is BombEquiped Bmb)
		{
			weaponToEquip.MagazineChange += ONMagazineChange;
			weaponToEquip.EmitMagazineChange(Bmb.clips, Bmb.MagazineSize);
		}



	}

	public void AddToInventory(PackedScene weaponScene, int slot)
	{
		if (slot >= 0 && slot < weaponInstances.Length)
		{
			// Clean up existing weapon in that slot if any
			if (weaponInstances[slot] != null)
			{
				weaponInstances[slot].QueueFree();
			}

			// Create new weapon instance
			var newWeapon = weaponScene.Instantiate<Weapon>();
			if (newWeapon != null)
			{
				newWeapon.MagazineChange += ONMagazineChange;
				weaponInstances[slot] = newWeapon;
			}
		}
	}

	public void SwitchWeapons(int weaponIndex)
	{
		if (weaponIndex != currentWeaponIndex && weaponInstances[weaponIndex] != null)
		{
			EquipWeapon(weaponIndex);

		}
	}



	private void ONMagazineChange(int ClipCount, int magazineCount)
	{

		hud.Text = $"{magazineCount}/{ClipCount}";


	}



	public override void _PhysicsProcess(double delta)
	{


		Vector3 velocity = Velocity;
		JumpVelocity = Mathf.Sqrt(2 * Strength * W_Gravity);
		if (Input.IsActionPressed("slot_1") && Dead == false)
		{
			SwitchWeapons(0);
		}
		else if (Input.IsActionPressed("slot_2") && Dead == false)
		{
			SwitchWeapons(1);
		}


		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta * Weight;
		}

		if (Input.IsActionJustPressed("Jump") && IsOnFloor() && Dead == false)
		{
			velocity.Y = JumpVelocity / Weight;
		}

		Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");
		Vector3 forward = camera.GlobalTransform.Basis.Z.Normalized();
		Vector3 right = camera.GlobalTransform.Basis.X.Normalized();
		Vector3 direction = (camera.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		bool isRunning = Input.IsActionPressed("Run");

		if (direction != Vector3.Zero && Dead == false)
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
		if (@event is InputEventMouseMotion R && Dead == false)
		{
			_yaw -= R.Relative.X * Sensitivity;
			_pitch -= R.Relative.Y * Sensitivity;

			_pitch = Mathf.Clamp(_pitch, MinPitch, MaxPitch);

			camera.RotationDegrees = new Vector3(_pitch, _yaw, 0);
		}



	}


}
