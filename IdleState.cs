using Godot;
using System;

public partial class IdleState : State
{
	// Called when the node enters the scene tree for the first time.
	[Export] public AnimationPlayer AnimMan;
	[Export] public float IdleTime;
	public override void Ready()
	{
		IdleTime = 2.0f;
	}

	public override void Update(float delta)
	{

		if (IdleTime > 0)
		{
			IdleTime -= delta;

		}
		else
		{
			Manager.TransitionTO("Wander");
		}
	}

	public override void Enter()
	{
		AnimMan.Play("Idle");
	}
	public override void Exit()
	{
		IdleTime = 2.0f;
		AnimMan.Stop();
	}
}

