using Godot;
using System;

public partial class Bullet2 : Node3D
{
	[Export]
	public float Speed { get; set; } = 50.0f;  // Speed of the bullet
	[Export]
	public float Lifetime { get; set; } = 5.0f;  // Lifetime of the bullet

	public Vector3 Direction { get; set; } = Vector3.Right;  // Default direction

	private Timer _timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer = new Timer();
		_timer.WaitTime = Lifetime;
		_timer.OneShot = true;
		_timer.Timeout += () => OnLifetimeTimeout();
		AddChild(_timer);
		_timer.Start();



	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Position += Direction * Speed * (float)delta;
		GD.Print(Position);



	}
	private void OnLifetimeTimeout()
	{
		QueueFree();  // Remove the bullet when its lifetime is over
	}

}
