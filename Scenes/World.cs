using Godot;
using System;

public partial class World : Node3D
{
	// Called when the node enters the scene tree for the first time.
	private GameMusicHandler DJ;
	public override void _Ready()
	{
		DJ = GetNode<GameMusicHandler>("/root/GameMusicHandler");
		var audioStream = (AudioStream)GD.Load("res://Music/BG/Mission.mp3");
		if (DJ.Player.Stream != audioStream)
		{
			DJ.Player.Stream = audioStream;
		}

		// Check if the audio is already playing before calling Play
		if (!DJ.Player.Playing)
		{
			DJ.Player.Play();
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
