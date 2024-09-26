using Godot;
using System;
using System.Collections.Generic;

public partial class FsmManager : Node
{
	// Called when the node enters the scene tree for the first time.
	[Export] public NodePath InitialState;
	[Export] public NodePath AnimeFsm;
	private Dictionary<string, State> states;
	private State CurrentState;
	public AnimationPlayer AnimationManager;
	public IdleState IdleState;
	[Export] public Area3D PlayerDetect;
	public CharacterBody3D NearestPlayer { get; private set; }
	public override void _Ready()
	{
		AnimationManager = GetNode<AnimationPlayer>(AnimeFsm);

		states = new Dictionary<string, State>();
		foreach (Node child in GetChildren())
		{
			if (child is State S)
			{
				states[child.Name] = S;
				S.Manager = this;
				S.Ready();
				S.Exit();

			}
		}


		CurrentState = GetNode<State>(InitialState);
		CurrentState.Enter();

		PlayerDetect.BodyEntered += EnterChase;
		PlayerDetect.BodyExited += ExitChase;
	}

	public void EnterChase(Node3D body)
	{


		if (body.IsInGroup("MC"))
		{
			UpdateNearestPlayer();

		}
	}

	// Function called when a player exits the Area3D
	public void ExitChase(Node3D body)
	{

		if (body.IsInGroup("MC"))
		{

			NearestPlayer = null;
		}
	}
	private void UpdateNearestPlayer()
	{


		foreach (Node3D body in PlayerDetect.GetOverlappingBodies())
		{
			if (body.IsInGroup("MC") && body is CharacterBody3D player)
			{
				NearestPlayer = null;
				float nearestDistance = float.MaxValue;
				float distance = PlayerDetect.GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
				if (distance < nearestDistance || distance == nearestDistance)
				{
					nearestDistance = distance;
					NearestPlayer = player;
				}
			}
		}

		if (NearestPlayer != null)
		{
			TransitionTO("Chase");
		}

	}

	public Vector3 getNearstPlayer()
	{
		return NearestPlayer.GlobalPosition;
	}


	public void TransitionTO(string Akey)
	{
		if (!states.ContainsKey(Akey) || CurrentState == states[Akey])
		{
			return;
		}
		CurrentState.Exit();
		CurrentState = states[Akey];
		CurrentState.Enter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CurrentState.Update((float)delta);
	}
	public override void _PhysicsProcess(double delta)
	{
		CurrentState.PhysicsUpdate((float)delta);
	}
}
