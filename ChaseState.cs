using Godot;
using System;
using System.Collections.Generic;

public partial class ChaseState : State
{
	private float speed = 5;
	[Export] public CharacterBody3D Enemy;
	[Export] public AnimationPlayer AnimMan;
	[Export] private Area3D hand;
	[Export] private Area3D Leg;
	private Node3D targetPlayer;
	public bool attacking;
	public bool canattack;
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

		if (Manager.NearestPlayer != null)
		{

			if (Enemy.IsOnFloor())
			{
				Vector3 Direction_Distance = Manager.getNearstPlayer() - Enemy.GlobalPosition;

				Vector3 Direction = Direction_Distance.Normalized();
				Direction.Y = 0;
				Direction = Direction.Normalized();
				Direction_Distance.Y = 0;
				float distanceok = Direction_Distance.Length();

				if (distanceok > 1.5 && canattack == true)
				{
					Enemy.Velocity = Direction * speed;
					Enemy.LookAt(Enemy.GlobalPosition - Direction, Vector3.Up);
					if (Enemy.Velocity.Length() > 0)
					{
						AnimMan.Play("Running");
					}
				}
				else if (distanceok <= 1.5)
				{
					AttackChoiceD = AttackChoice.RandiRange(0, 1);
					if (canattack == true)
					{
						attacking = false;
					}
					if (AttackChoiceD == 0 && attacking == false)
					{
						AnimMan.Play("punching");
					}
					else if (AttackChoiceD == 1 && attacking == false)
					{
						AnimMan.Play("Kicking");
					}


					Enemy.Velocity = new Vector3(0, 0, 0);
				}



			}
			else
			{
				Vector3 direction = (Manager.getNearstPlayer() - Enemy.GlobalPosition).Normalized();
				Enemy.Velocity = speed * Enemy.GetGravity();
				Enemy.LookAt(Enemy.GlobalPosition - direction, Vector3.Up);
				if (Enemy.Velocity.Length() > 0)
				{
					AnimMan.Play("Running");
				}

			}
		}
		else if (Manager.NearestPlayer == null)
		{
			Enemy.Velocity = new Vector3(0, 0, 0);
			Enemy.MoveAndSlide();
			Manager.TransitionTO("Idle");
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


}

