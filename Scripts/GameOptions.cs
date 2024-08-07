using Godot;
using System;

public partial class GameOptions : Node
{
	public float M_Slider;
	public float BGM_Slider;
	public float ES_Slider;
	public override void _Ready()
	{
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


}

