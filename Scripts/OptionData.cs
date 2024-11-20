using Godot;
using System;
[GlobalClass]
public partial class OptionData : Resource
{
	[Export] public float M_Slider = 0;
	[Export] public float BGM_Slider = 0;
	[Export] public float ES_Slider = 0;
	[Export] public int slide_button = 0;
	[Export] public int Button_state = 0;
	[Export] public int WindowIndex = 0;
	[Export] public float FovSlide = 75;
	[Export] public float Sensitivity = 100;
	[Export] public string LangLocale = "en";



	public void SaveAudioSetting(float m, int idx)
	{

		if (idx == 0)
		{
			M_Slider = m;
			AudioServer.SetBusVolumeDb(0, Mathf.LinearToDb(m));
		}
		else if (idx == 1)
		{
			BGM_Slider = m;
			AudioServer.SetBusVolumeDb(1, Mathf.LinearToDb(m));
		}
		else if (idx == 2)
		{
			ES_Slider = m;
			AudioServer.SetBusVolumeDb(2, Mathf.LinearToDb(m));
		}

	}
	public void ButtonStateToggled(int x)
	{
		Button_state = x;
	}
	public void saveresolution(int x)
	{
		slide_button = x;
	}
	public void ScreenWindowinit(int x)
	{
		if (x == 0)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else if (x == 1)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			DisplayServer.WindowSetSize(new Vector2I(1280, 720));
		}


	}
	public void LangInit(string LanguageCode)
	{

		TranslationServer.SetLocale(LanguageCode);

	}

}
