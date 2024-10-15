using Godot;
using System;



public partial class Zombie1 : CharacterBody3D
{

	private Skeleton3D skele;
	[Export] private PackedScene dragdoll;

	private int health = 10;
	private AnimationPlayer Anim;
	private GameDAta _gdata;
	private int score;

	private void _LoadData(GameDAta data)
	{
		if (data == null)
		{
			GD.PrintErr("Failed to load OptionData. Data is null.");
			return;
		}
		_gdata = data;


	}
	private void RuntimeLoad()
	{
		string fileName = "res://Resources/GameData.tres";
		if (ResourceLoader.Exists(fileName))
		{

			_LoadData(ResourceLoader.Load<GameDAta>(fileName, null, ResourceLoader.CacheMode.Ignore));

		}
		else
		{
			GD.PrintErr("Options.tres file does not exist.");
		}

	}
	private void SaveData()
	{
		string fileName = "res://Resources/GameData.tres";
		ResourceSaver.Save(_gdata, fileName);

	}

	public override void _Ready()
	{
		skele = GetNode<Skeleton3D>("Armature/Skeleton3D");
		Anim = GetNode<AnimationPlayer>("AnimationPlayer");



	}
	private void recieve_damage(int x)
	{
		health -= x;
		GD.Print(health);
		if (health <= 0)
		{
			RuntimeLoad();
			_gdata.Score += 5;
			var Rag = dragdoll.Instantiate<Node3D>();
			Rag.Position = this.GlobalPosition;
			QueueFree();
			GetTree().Root.AddChild(Rag);
			SaveData();
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;


		velocity += GetGravity() * (float)delta;






		Velocity = velocity;
		MoveAndSlide();



	}


}
