using Godot;
using System;

public partial class BulletRigidPhysics : RigidBody3D
{

	[Export]
	private float LifeTime = 60f;
	private float damage = 0;
	private Timer Lifetime;
	private bool damaged = false;
	public override void _Ready()
	{// Example radius of the bullet in meters
		Lifetime = GetNode<Timer>("Timer");
		Lifetime.WaitTime = LifeTime;
		Lifetime.Start();
		Lifetime.Timeout += () => QueueFree();
		this.BodyEntered += OnEnemyDetect;
	}
	private void OnEnemyDetect(Node body)
	{



		if (body.IsInGroup("Zombies"))
		{
			var par = body.GetParent().GetParent().GetParent().GetParent();
			var part = body.GetParent();
			var source = this.GlobalTransform.Origin;

			if (part.Name == "Head")
			{


				damage = 10;
				par.Call("recieve_damage", damage, 350, source);
				damaged = true;
				QueueFree();

			}
			else if (part.Name != "Head")
			{
				damage = 1;
				par.Call("recieve_damage", damage, 1, source);
				damaged = true;
				QueueFree();
			}
		}





	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

	}
}
