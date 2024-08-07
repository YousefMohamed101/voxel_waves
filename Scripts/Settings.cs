using Godot;
using System;

public partial class Settings : Node
{



	public void SetResolution(int idx)
	{
		if (idx == 0)
		{
			ProjectSettings.SetSetting("display/window/size/viewport_width", "1280");
			ProjectSettings.SetSetting("display/window/size/viewport_height", "720");
		}
		else if (idx == 1)
		{
			ProjectSettings.SetSetting("display/window/size/viewport_width", "1280");
			ProjectSettings.SetSetting("display/window/size/viewport_height", "1024");
		}
		else if (idx == 2)
		{
			ProjectSettings.SetSetting("display/window/size/viewport_width", "1366");
			ProjectSettings.SetSetting("display/window/size/viewport_height", "768");
		}
		else if (idx == 3)
		{
			ProjectSettings.SetSetting("display/window/size/viewport_width", "1536");
			ProjectSettings.SetSetting("display/window/size/viewport_height", "864");
		}
		else if (idx == 4)
		{
			ProjectSettings.SetSetting("display/window/size/viewport_width", "1600");
			ProjectSettings.SetSetting("display/window/size/viewport_height", "900");
		}
		else if (idx == 5)
		{
			ProjectSettings.SetSetting("display/window/size/viewport_width", "1920");
			ProjectSettings.SetSetting("display/window/size/viewport_height", "1080");
		}
		else if (idx == 6)
		{
			ProjectSettings.SetSetting("display/window/size/viewport_width", "2540");
			ProjectSettings.SetSetting("display/window/size/viewport_height", "1440");
		}
	}
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

	public override void _Ready()
	{

		OptionCarry = GetNode<GameOptions>("/root/GameOptions");
		MSlider = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/Master");
		BGMSlider = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/BGM");
		SESlider = GetNode<HSlider>("CenterContainer/VBoxContainer/GridContainer/Effects");
		MSlider.Value = OptionCarry.M_Slider;
		BGMSlider.Value = OptionCarry.BGM_Slider;
		SESlider.Value = OptionCarry.ES_Slider;
		MSlider.ValueChanged += OnSliderValueChanged;
		BGMSlider.ValueChanged += OnBGMValueChanged;
		SESlider.ValueChanged += OnSEValueChanged;
		GetNode<Button>("CenterContainer/VBoxContainer/Back").Pressed += () => GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("MainMenu.tscn");
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
