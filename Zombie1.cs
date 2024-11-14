using Godot;
using System;



public partial class Zombie1 : CharacterBody3D
{

	private Skeleton3D skele;
	[Export] private PackedScene dragdoll;

	private int health = 10;
	private AnimationPlayer Anim;

	private World world;
	private int score;




	public override void _Ready()
	{
		skele = GetNode<Skeleton3D>("Armature/Skeleton3D");
		Anim = GetNode<AnimationPlayer>("AnimationPlayer");
		world = GetNode<World>("/root/World");



	}
	private void recieve_damage(int x, int y, Vector3 source)
	{
		health -= x;
		GD.Print(health);
		if (health <= 0)
		{

			world.scoreupdate(5);
			RigidBody3D Rag = dragdoll.Instantiate<RigidBody3D>();
			Rag.Position = this.GlobalPosition;
			Rag.ApplyCentralForce((GlobalTransform.Origin - source).Normalized() * y);
			QueueFree();
			GetTree().Root.AddChild(Rag);

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
