using Godot;
using System;

public partial class GameOptions : Node
{
	public float M_Slider;
	public float BGM_Slider;
	public float ES_Slider;
	public int slide_button;
	public int Button_state;
	public int Checked_state;
	private int count;

	public override void _Ready()
	{
		Checked_state = 0;
		count = 0;
		Button_state = 0;
		slide_button = 0;
		M_Slider = 0;
		BGM_Slider = 0;
		ES_Slider = 0;

	}
	public void SaveInfoSetting(float m, int idx)
	{

		if (idx == 0)
		{
			M_Slider = m;
			AudioServer.SetBusVolumeDb(0, m);
		}
		else if (idx == 1)
		{
			BGM_Slider = m;
			AudioServer.SetBusVolumeDb(1, m);
		}
		else if (idx == 2)
		{
			ES_Slider = m;
			AudioServer.SetBusVolumeDb(2, m);
		}

	}
	public void ButtonStateToggled(int x)
	{
		Button_state = x;
	}
	public void ButtonCheckedMe()
	{
		if (Checked_state == 0)
		{
			Checked_state = 1;
		}
		else
		{
			Checked_state = 0;
		}


		switch (Checked_state)
		{
			case 0:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
				GD.Print("windowed");

				break;

			case 1:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
				GD.Print("fullscreen");
				break;

			default:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
				break;
		}

	}


}

