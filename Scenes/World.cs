using Godot;
using System;

public partial class World : Node3D
{
	// Called when the node enters the scene tree for the first time.
	private GameMusicHandler DJ;
	private Timer EnTimer;
	private Node3D EnSpawn;
	private CenterContainer InGameMenu;
	private ColorRect MenuShade;
	private GameDAta _gdata;
	private int score;
	private Label lableScore;
	private World world;
	[Signal]
	public delegate void ScoreChangedEventHandler(int newScore);

	private void _LoadData(GameDAta data)
	{
		if (data == null)
		{
			GD.PrintErr("Failed to load OptionData. Data is null.");
			return;
		}
		_gdata = data;


	}
	private void RuntimeLoad()
	{
		string fileName = "res://Resources/GameData.tres";
		if (ResourceLoader.Exists(fileName))
		{

			_LoadData(ResourceLoader.Load<GameDAta>(fileName, null, ResourceLoader.CacheMode.Ignore));

		}
		else
		{
			GD.PrintErr("Options.tres file does not exist.");
		}

	}
	private void SaveData()
	{
		string fileName = "res://Resources/GameData.tres";
		ResourceSaver.Save(_gdata, fileName);

	}

	public override void _Ready()
	{
		RuntimeLoad();
		_gdata.Score = 0;
		_gdata.deads = false;
		SaveData();
		DJ = GetNode<GameMusicHandler>("/root/GameMusicHandler");
		InGameMenu = GetNode<CenterContainer>("Unpause/CenterContainer");
		MenuShade = GetNode<ColorRect>("Unpause/ColorRect");
		lableScore = GetNode<Label>("Unpause/Score");
		score = 0;
		scoreupdate(score);
		MenuShade.Visible = false;
		InGameMenu.Visible = false;


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



	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey e && e.IsPressed() && e.Keycode == Key.Escape)
		{
			GetTree().Paused = !GetTree().Paused;
			if (DisplayServer.MouseGetMode() == DisplayServer.MouseMode.Captured)
			{
				DisplayServer.MouseSetMode(DisplayServer.MouseMode.Visible);
				InGameMenu.Visible = true;
				MenuShade.Visible = true;
			}


			GetTree().Paused = true;


		}
	}


	public void scoreupdate(int x)
	{
		score += x;
		lableScore.Text = $"{score}";
		_gdata.Score = score;
		SaveData();
	}
	public override void _PhysicsProcess(double delta)
	{

	}
}
