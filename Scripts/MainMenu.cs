using Godot;
using System;

public partial class MainMenu : Node
{
	private GameMusicHandler DJ;
	private GameDAta Highscore;
	private Label highscorelabel;
	public override void _Ready()
	{
		RuntimeLoad();
		GetTree().Paused = false;
		GetNode<Button>("Play").Pressed += () => GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("GameLoading.tscn");
		GetNode<Button>("Setting").Pressed += () => GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("Settings.tscn");
		GetNode<Button>("Quit").Pressed += () => GetTree().Quit();
		DJ = GetNode<GameMusicHandler>("/root/GameMusicHandler");
		var audioStream = (AudioStream)GD.Load("res://Music/BG/Calamity.wav");
		highscorelabel = GetNode<Label>("Highscore");
		highscorelabel.Text = $"HighScore: {Highscore.Highscore}";
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
	private void _LoadData(GameDAta data)
	{
		Highscore = data;

	}
	private void RuntimeLoad()
	{
		string fileName = "res://Resources/GameData.tres";
		if (ResourceLoader.Exists(fileName))
		{

			_LoadData(ResourceLoader.Load<GameDAta>(fileName, null, ResourceLoader.CacheMode.Ignore));
		}

	}


}
