using Godot;
using System;

public partial class ResetResources : Button
{
	private OptionData _data;
	private GameDAta _data2;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RuntimeLoad();
		this.Pressed += Reset;
	}
	private void _LoadData(OptionData data)
	{
		_data = data;
	}
	private void _LoadData2(GameDAta data)
	{
		_data2 = data;
	}



	private void RuntimeLoad()
	{
		string fileName = "res://Resources/Options.tres";
		string fileName2 = "res://Resources/GameData.tres";
		if (ResourceLoader.Exists(fileName))
		{

			_LoadData(ResourceLoader.Load<OptionData>(fileName, null, ResourceLoader.CacheMode.Ignore));
			_LoadData2(ResourceLoader.Load<GameDAta>(fileName2, null, ResourceLoader.CacheMode.Ignore));
		}

	}
	private void SaveData()
	{
		string fileName = "res://Resources/Options.tres";
		string fileName2 = "res://Resources/GameData.tres";
		ResourceSaver.Save(_data, fileName);
		ResourceSaver.Save(_data2, fileName2);

	}

	private void Reset()
	{
		_data.BGM_Slider = 1;
		_data.ES_Slider = 1;
		_data.M_Slider = 1;
		_data.FovSlide = 75;
		_data.Sensitivity = 1;
		_data2.Highscore = 0;
		_data2.Score = 0;
		SaveData();
	}
	public override void _Process(double delta)
	{
	}
}
