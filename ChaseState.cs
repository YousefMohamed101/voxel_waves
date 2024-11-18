using Godot;
using System;
using System.Collections.Generic;

public partial class ChaseState : State
{
	private float speed = 10;
	private float separationRadius = 5.0f; // Minimum distance between enemies
	private float separationWeight = 0.5f;

	[Export] public CharacterBody3D Enemy;
	[Export] public AnimationPlayer AnimMan;
	[Export] private Area3D hand;
	[Export] private Area3D Leg;
	private Node3D targetPlayer;
	public bool attacking;
	public bool canattack;
	[Export] public NavigationAgent3D Tracker;
	private RandomNumberGenerator AttackChoice;
	private int AttackChoiceD;
	public override void Enter()
	{
		GD.Print("chase");
		attacking = false;

	}
	public override void Exit()
	{
		targetPlayer = null;

		GD.Print("chaseout");
	}
	public override void Ready()
	{
		AttackChoice = new RandomNumberGenerator();
		hand.BodyEntered += applydamage;
		Leg.BodyEntered += applydamage;
		AnimMan.AnimationFinished += OnAnimationFinished;
		AnimMan.AnimationStarted += OnAnimationStarted;
		canattack = true;



	}
	public override void Update(float delta)
	{


	}
	public override void PhysicsUpdate(float delta)
	{

		if (Manager.NearestPlayer == null)
		{
			StopEnemy();
			Manager.TransitionTO("Idle");
			return;
		}

		if (!Enemy.IsOnFloor()) // Handle air motion
		{
			HandleAirMotion();
			return;
		}

		Vector3 targetDirection = (Manager.getNearstPlayer() - Enemy.GlobalPosition).Normalized();
		targetDirection.Y = 0;

		float distance = (Manager.getNearstPlayer() - Enemy.GlobalPosition).Length();
		if (distance > 1.5f && canattack)
		{
			HandleChase(targetDirection);
		}
		else if (distance <= 1.5f)
		{
			HandleAttack();
		}
	}

	private void applydamage(Node3D body)
	{


		if (body.IsInGroup("MC") && attacking == true)
		{
			body.Call("recieve_damage", 1);
			GD.Print("hit");

		}
	}
	private void OnAnimationFinished(StringName anim_name)
	{

		if (anim_name == "punching" || anim_name == "Kicking")
		{
			attacking = false;
			canattack = true;
		}
	}
	private void OnAnimationStarted(StringName anim_name)
	{

		if (anim_name == "punching" || anim_name == "Kicking")
		{
			attacking = true;
			canattack = false;
		}
		if (anim_name == "Running")
		{
			canattack = true;
		}
	}


	private void HandleChase(Vector3 direction)
	{


		Enemy.Velocity = direction * speed;
		Enemy.LookAt(Enemy.GlobalPosition - direction, Vector3.Up);

		if (Enemy.Velocity.Length() > 0)
		{
			AnimMan.SpeedScale = 1.5f;
			AnimMan.Play("Running");
		}
	}

	private void HandleAttack()
	{
		if (canattack)
		{
			int attackChoiceD = AttackChoice.RandiRange(0, 1);
			AnimMan.SpeedScale = 2f;

			if (attackChoiceD == 0 && !attacking)
			{
				AnimMan.Play("punching");
			}
			else if (attackChoiceD == 1 && !attacking)
			{
				AnimMan.Play("Kicking");
			}

			attacking = true;
			canattack = false;
		}

		Enemy.Velocity = Vector3.Zero;
	}

	private void HandleAirMotion()
	{
		Vector3 direction = (Manager.getNearstPlayer() - Enemy.GlobalPosition).Normalized();
		Enemy.Velocity = speed * Enemy.GetGravity();
		Enemy.LookAt(Enemy.GlobalPosition - direction, Vector3.Up);

		if (Enemy.Velocity.Length() > 0)
		{
			AnimMan.Play("Running");
		}
	}

	private void StopEnemy()
	{
		Enemy.Velocity = Vector3.Zero;
		Enemy.MoveAndSlide();
	}


}

