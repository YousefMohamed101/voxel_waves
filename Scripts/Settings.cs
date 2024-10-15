using Godot;
using System;

public partial class Settings : Node
{







	private HSlider MSlider;
	private HSlider BGMSlider;
	private HSlider SESlider;
	private HSlider Fov;

	private OptionButton Slider_Button;

	private Button B_en;
	private Button B_fr;
	private Button B_es;
	private CheckButton B_check;
	private int windowMode;
	private OptionData _data;
	private int button_local_pressed;
	private HSlider sense;

	public override void _Ready()
	{
		MSlider = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/Master");
		BGMSlider = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/BGM");
		SESlider = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/Effects");
		Slider_Button = GetNode<OptionButton>("CenterContainer/VBoxContainer/GridContainer/OptionButton");
		B_en = GetNode<Button>("CenterContainer/VBoxContainer/GridContainer/HBoxContainer/EN");
		B_fr = GetNode<Button>("CenterContainer/VBoxContainer/GridContainer/HBoxContainer/FR");
		B_es = GetNode<Button>("CenterContainer/VBoxContainer/GridContainer/HBoxContainer/ES");
		B_check = GetNode<CheckButton>("CenterContainer/VBoxContainer/GridContainer/CheckButton");
		Fov = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/Fov2");
		sense = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/Sense");
		B_en.Pressed += () => langselected(0);
		B_fr.Pressed += () => langselected(1);
		B_es.Pressed += () => langselected(2);
		RuntimeLoad();
		switch (button_local_pressed)
		{
			case 0:
				B_en.ButtonPressed = true;
				break;
			case 1:
				B_fr.ButtonPressed = true;
				break;
			case 2:
				B_es.ButtonPressed = true;
				break;
			default:
				B_en.ButtonPressed = true;
				break;
		}

		B_check.Toggled += OnFullscreenToggled;
		if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
		{
			B_check.ButtonPressed = true; // Ensure CheckButton reflects Fullscreen state
		}
		else
		{
			B_check.ButtonPressed = false; // Ensure CheckButton reflects Windowed state
		}

		Slider_Button.ItemSelected += OnOptionButtonItemSelected;
		MSlider.ValueChanged += OnSliderValueChanged;
		BGMSlider.ValueChanged += OnBGMValueChanged;
		SESlider.ValueChanged += OnSEValueChanged;
		Fov.ValueChanged += FovManag;
		sense.ValueChanged += SenseSet;
		GetNode<Button>("CenterContainer/VBoxContainer/Back").Pressed += backbuttonlife;
	}

	private void _LoadData(OptionData data)
	{
		_data = data;
		Slider_Button.Selected = _data.slide_button;
		MSlider.Value = _data.M_Slider;
		BGMSlider.Value = _data.BGM_Slider;
		SESlider.Value = _data.ES_Slider;
		button_local_pressed = _data.Button_state;
		windowMode = _data.WindowIndex;
		Fov.Value = _data.FovSlide;
		sense.Value = _data.Sensitivity;


	}
	private void RuntimeLoad()
	{
		string fileName = "res://Resources/Options.tres";
		if (ResourceLoader.Exists(fileName))
		{

			_LoadData(ResourceLoader.Load<OptionData>(fileName, null, ResourceLoader.CacheMode.Ignore));
		}

	}
	private void SaveData()
	{
		string fileName = "res://Resources/Options.tres";
		ResourceSaver.Save(_data, fileName);

	}
	private void FovManag(double y)
	{

		_data.FovSlide = (float)y;
	}
	private void SenseSet(double y)
	{

		_data.Sensitivity = (float)y;
	}


	private void backbuttonlife()
	{
		SaveData();
		GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("MainMenu.tscn");

	}
	public void _ToggleLanguage(bool toggleOn, string LanguageCode)
	{
		if (toggleOn)
		{
			TranslationServer.SetLocale(LanguageCode);
			_data.LangLocale = LanguageCode;
		}
	}
	public void langselected(int x)
	{
		_data.Button_state = x;
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
}
