using Godot;
using System;

public partial class MainMenu : Node
{
	private GameMusicHandler DJ;
	public override void _Ready()
	{
		GetTree().Paused = false;
		GetNode<Button>("Play").Pressed += () => GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("GameLoading.tscn");
		GetNode<Button>("Setting").Pressed += () => GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("Settings.tscn");
		GetNode<Button>("Quit").Pressed += () => GetTree().Quit();
		DJ = GetNode<GameMusicHandler>("/root/GameMusicHandler");
		var audioStream = (AudioStream)GD.Load("res://Music/BG/Calamity.wav");
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


}
