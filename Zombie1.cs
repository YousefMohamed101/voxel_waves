using Godot;
using System;



public partial class Zombie1 : CharacterBody3D
{
	public enum States
	{
		idle,
		Wander,
		Chassing,
		Attacking
	}

	private Skeleton3D skele;
	[Export] private PackedScene dragdoll;

	private int health = 10;
	private AnimationPlayer Anim;

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


			var Rag = dragdoll.Instantiate<Node3D>();
			Rag.Position = this.GlobalPosition;
			QueueFree();
			GetTree().Root.AddChild(Rag);


		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;


		velocity += GetGravity() * (float)delta;






		Velocity = velocity;
		MoveAndCollide(Velocity * (float)delta);



	}


}
