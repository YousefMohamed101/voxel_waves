using Godot;
using System;

public partial class Unpause : CenterContainer
{
	// Called when the node enters the scene tree for the first time.
	private CenterContainer InGameMenu;
	private Button Resume;
	private Button Quit;
	private Button Mainemenu;
	private HSlider Fov;
	private HSlider BGM;
	private HSlider SE;
	private HSlider Master;
	private HSlider sense;
	private CheckButton Fullcheck;
	private OptionButton Res;
	private OptionData _data = ResourceLoader.Load<OptionData>("res://Resources/Options.tres", null, ResourceLoader.CacheMode.Ignore);
	private int intcheck;
	private int windowMode;
	private Player Charplayer;
	private Camera3D CharCamera;

	public override void _Ready()
	{

		Fullcheck = GetNode<CheckButton>("VBoxContainer/GridContainer/CheckButton");
		Charplayer = GetNode<Player>("/root/World/Player");
		CharCamera = GetNode<Camera3D>("/root/World/Player/Camera3D");

		Res = GetNode<OptionButton>("VBoxContainer/GridContainer/OptionButton");
		Resume = GetNode<Button>("VBoxContainer/Resume");
		Mainemenu = GetNode<Button>("VBoxContainer/Back_to_main_menu");
		Quit = GetNode<Button>("VBoxContainer/Quit_Game");
		Fov = GetNode<HSlider>("VBoxContainer/GridContainer/Fov2");
		BGM = GetNode<HSlider>("VBoxContainer/GridContainer/BGM");
		SE = GetNode<HSlider>("VBoxContainer/GridContainer/Effects");
		Master = GetNode<HSlider>("VBoxContainer/GridContainer/Master");
		sense = GetNode<HSlider>("VBoxContainer/GridContainer/Sense");
		RuntimeLoad();
		Fov.ValueChanged += FovManag;
		Fullcheck.Toggled += OnFullscreenToggled;
		Res.ItemSelected += OnOptionButtonItemSelected;
		Resume.Pressed += ResumeGame;
		Master.ValueChanged += OnSliderValueChanged;
		BGM.ValueChanged += OnBGMValueChanged;
		SE.ValueChanged += OnSEValueChanged;
		Quit.Pressed += Quit_Game;
		Mainemenu.Pressed += Back_to_main_menu;
	}
	private void ResumeGame()
	{
		GetTree().Paused = false;
		this.Visible = false;
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
		SaveData();
	}
	private void Back_to_main_menu()
	{
		SaveData();
		GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("MainMenu.tscn");
	}
	private void Quit_Game()
	{
		SaveData();
		GetTree().Quit();
	}
	private void _LoadData(OptionData data)
	{
		if (data == null)
		{
			GD.PrintErr("Failed to load OptionData. Data is null.");
			return;
		}
		_data = data;
		if (Res != null) Res.Selected = _data.slide_button;
		if (Master != null) Master.Value = _data.M_Slider;
		if (BGM != null) BGM.Value = _data.BGM_Slider;
		if (SE != null) SE.Value = _data.ES_Slider;
		windowMode = _data.WindowIndex;
		if (Fov != null) Fov.Value = _data.FovSlide;
		if (sense != null) sense.Value = _data.Sensitivity;

	}
	private void RuntimeLoad()
	{
		string fileName = "res://Resources/Options.tres";
		if (ResourceLoader.Exists(fileName))
		{

			_LoadData(ResourceLoader.Load<OptionData>(fileName, null, ResourceLoader.CacheMode.Ignore));

		}
		else
		{
			GD.PrintErr("Options.tres file does not exist.");
		}

	}
	private void FovManag(double y)
	{
		CharCamera.Fov = (float)y;
		_data.FovSlide = (float)y;
	}
	private void SenseSet(double y)
	{
		Charplayer.Sensitivity = (float)y;
		_data.Sensitivity = (float)y;
	}


	private void SaveData()
	{
		string fileName = "res://Resources/Options.tres";
		ResourceSaver.Save(_data, fileName);

	}
	private void OnOptionButtonItemSelected(long index)
	{
		if (index == 0)
		{
			DisplayServer.WindowSetSize(new Vector2I(1280, 270));
			_data.slide_button = 0;
		}
		else if (index == 1)
		{
			DisplayServer.WindowSetSize(new Vector2I(1280, 1024));
			_data.slide_button = 1;
		}
		else if (index == 2)
		{
			DisplayServer.WindowSetSize(new Vector2I(1366, 768));
			_data.slide_button = 2;
		}
		else if (index == 3)
		{
			DisplayServer.WindowSetSize(new Vector2I(1536, 864));
			_data.slide_button = 3;
		}
		else if (index == 4)
		{
			DisplayServer.WindowSetSize(new Vector2I(1600, 900));
			_data.slide_button = 4;
		}
		else if (index == 5)
		{
			DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
			_data.slide_button = 5;
		}
		else if (index == 6)
		{
			DisplayServer.WindowSetSize(new Vector2I(2540, 1440));
			_data.slide_button = 6;
		}
	}
	private void OnSliderValueChanged(double value)
	{
		_data.SaveAudioSetting((float)value, 0);
	}
	private void OnBGMValueChanged(double value)
	{
		_data.SaveAudioSetting((float)value, 1);
	}
	private void OnSEValueChanged(double value)
	{
		_data.SaveAudioSetting((float)value, 2);
	}
	private void OnFullscreenToggled(bool buttonPressed)
	{
		if (buttonPressed)
		{

			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			windowMode = 0;
			_data.WindowIndex = windowMode;
		}
		else
		{

			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			DisplayServer.WindowSetSize(new Vector2I(1280, 720));
			windowMode = 1;
			_data.WindowIndex = windowMode;
		}
	}



}
