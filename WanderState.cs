using Godot;
using System;

public partial class WanderState : State
{
	[Export] public AnimationPlayer AnimMan;
	[Export] public CharacterBody3D Enemy;
	private float wanderTime;
	private GridMap gridMap;
	private RandomNumberGenerator NumberRando;
	private Vector3 WanderDirection;
	private float speed = 10.0f;

	public override void Ready()
	{
		gridMap = GetNode<GridMap>("/root/World/Land");
		wanderTime = 1f;
		NumberRando = new RandomNumberGenerator();
	}

	public override void Enter()
	{
		WanderLocation();


	}
	public override void Update(float delta)
	{
		if (wanderTime > 0)
		{
			wanderTime -= delta;


		}
		else
		{
			Manager.TransitionTO("Idle");
		}
	}
	private void WanderLocation()
	{
		NumberRando.Randomize();

		WanderDirection = new Vector3((float)NumberRando.RandfRange(-1, 1), 0, (float)NumberRando.RandfRange(-1, 1)).Normalized();
		wanderTime = NumberRando.RandfRange(1, 5);
	}
	public override void PhysicsUpdate(float delta)
	{
		if (Enemy != null)
		{
			if (!Enemy.IsOnFloor())
			{
				Enemy.Velocity = WanderDirection * speed * Enemy.GetGravity();
				Enemy.LookAt(Enemy.GlobalPosition - WanderDirection, Vector3.Up);
				if (Enemy.Velocity.Length() > 0)
				{
					AnimMan.Play("Running");
				}

			}
			else
			{
				Enemy.Velocity = WanderDirection * speed;
				Enemy.LookAt(Enemy.GlobalPosition - WanderDirection, Vector3.Up);
				if (Enemy.Velocity.Length() > 0)
				{
					AnimMan.Play("Running");
				}

			}
		}

	}

	public override void Exit()
	{
		AnimMan.Stop();
		Enemy.Velocity = new Vector3(0, 0, 0);
	}


}
