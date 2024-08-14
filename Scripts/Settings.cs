using Godot;
using System;

public partial class Settings : Node
{



	public void _ToggleLanguage(bool toggleOn, string LanguageCode)
	{
		if (toggleOn)
		{
			TranslationServer.SetLocale(LanguageCode);
		}
	}


	private GameOptions OptionCarry;
	private HSlider MSlider;
	private HSlider BGMSlider;
	private HSlider SESlider;
	private OptionButton Slider_Button;

	private Button B_en;
	private Button B_fr;
	private Button B_es;
	private CheckButton B_check;
	public override void _Ready()
	{

		OptionCarry = GetNode<GameOptions>("/root/GameOptions");
		MSlider = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/Master");
		BGMSlider = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/BGM");
		SESlider = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/Effects");
		Slider_Button = GetNode<OptionButton>("CenterContainer/VBoxContainer/GridContainer/OptionButton");
		B_en = GetNode<Button>("CenterContainer/VBoxContainer/GridContainer/HBoxContainer/EN");
		B_fr = GetNode<Button>("CenterContainer/VBoxContainer/GridContainer/HBoxContainer/FR");
		B_es = GetNode<Button>("CenterContainer/VBoxContainer/GridContainer/HBoxContainer/ES");
		B_check = GetNode<CheckButton>("CenterContainer/VBoxContainer/GridContainer/CheckButton");
		B_en.Pressed += () => OptionCarry.ButtonStateToggled(0);
		B_fr.Pressed += () => OptionCarry.ButtonStateToggled(1);
		B_es.Pressed += () => OptionCarry.ButtonStateToggled(2);
		switch (OptionCarry.Button_state)
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

		B_check.Pressed += () => OptionCarry.ButtonCheckedMe();
		if (OptionCarry.Checked_state == 1)
		{
			B_check.ButtonPressed = true;
		}
		Slider_Button.Selected = OptionCarry.slide_button;
		Slider_Button.ItemSelected += OnOptionButtonItemSelected;
		MSlider.Value = OptionCarry.M_Slider;
		BGMSlider.Value = OptionCarry.BGM_Slider;
		SESlider.Value = OptionCarry.ES_Slider;
		MSlider.ValueChanged += OnSliderValueChanged;
		BGMSlider.ValueChanged += OnBGMValueChanged;
		SESlider.ValueChanged += OnSEValueChanged;
		GetNode<Button>("CenterContainer/VBoxContainer/Back").Pressed += () => GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("MainMenu.tscn");
	}


	private void OnOptionButtonItemSelected(long index)
	{
		if (index == 0)
		{
			DisplayServer.WindowSetSize(new Vector2I(1280, 270));
			OptionCarry.slide_button = 0;
		}
		else if (index == 1)
		{
			DisplayServer.WindowSetSize(new Vector2I(1280, 1024));
			OptionCarry.slide_button = 1;
		}
		else if (index == 2)
		{
			DisplayServer.WindowSetSize(new Vector2I(1366, 768));
			OptionCarry.slide_button = 2;
		}
		else if (index == 3)
		{
			DisplayServer.WindowSetSize(new Vector2I(1536, 864));
			OptionCarry.slide_button = 3;
		}
		else if (index == 4)
		{
			DisplayServer.WindowSetSize(new Vector2I(1600, 900));
			OptionCarry.slide_button = 4;
		}
		else if (index == 5)
		{
			DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
			OptionCarry.slide_button = 5;
		}
		else if (index == 6)
		{
			DisplayServer.WindowSetSize(new Vector2I(2540, 1440));
			OptionCarry.slide_button = 6;
		}
	}

	private void OnSliderValueChanged(double value)
	{
		OptionCarry.SaveInfoSetting((float)value, 0);
	}
	private void OnBGMValueChanged(double value)
	{
		OptionCarry.SaveInfoSetting((float)value, 1);
	}
	private void OnSEValueChanged(double value)
	{
		OptionCarry.SaveInfoSetting((float)value, 2);
	}
}
