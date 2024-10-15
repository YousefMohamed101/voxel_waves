using Godot;
using System;

public partial class SplashScreen : ColorRect
{
	private TextureRect logo_A;
	private AnimationPlayer Anim;
	private GameMusicHandler DJ;
	private OptionData _data;
	public override void _Ready()
	{
		RuntimeLoad();
		Anim = GetNode<AnimationPlayer>("CenterContainer/AnimationPlayer");
		GetNode<Timer>("Timer").Timeout += () => GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("MainMenu.tscn");
		Anim.Play("FadeIn");
		DJ = GetNode<GameMusicHandler>("/root/GameMusicHandler");
		DJ.Player.Stream = (AudioStream)GD.Load("res://Music/BG/IntroMusic.mp3");
		DJ.Player.Play();

	}
	private void _LoadData(OptionData data)
	{
		_data = data;
		OnOptionButtonItemSelected(_data.slide_button);
		_data.SaveAudioSetting(_data.M_Slider, 0);
		_data.SaveAudioSetting(_data.BGM_Slider, 1);
		_data.SaveAudioSetting(_data.ES_Slider, 2);
		_data.ScreenWindowinit(_data.WindowIndex);
		_data.LangInit(_data.LangLocale);
	}



	private void RuntimeLoad()
	{
		string fileName = "res://Resources/Options.tres";
		if (ResourceLoader.Exists(fileName))
		{

			_LoadData(ResourceLoader.Load<OptionData>(fileName, null, ResourceLoader.CacheMode.Ignore));
		}

	}
	private void OnOptionButtonItemSelected(long index)
	{
		if (index == 0)
		{
			DisplayServer.WindowSetSize(new Vector2I(1280, 270));

		}
		else if (index == 1)
		{
			DisplayServer.WindowSetSize(new Vector2I(1280, 1024));

		}
		else if (index == 2)
		{
			DisplayServer.WindowSetSize(new Vector2I(1366, 768));

		}
		else if (index == 3)
		{
			DisplayServer.WindowSetSize(new Vector2I(1536, 864));

		}
		else if (index == 4)
		{
			DisplayServer.WindowSetSize(new Vector2I(1600, 900));

		}
		else if (index == 5)
		{
			DisplayServer.WindowSetSize(new Vector2I(1920, 1080));

		}
		else if (index == 6)
		{
			DisplayServer.WindowSetSize(new Vector2I(2540, 1440));

		}
	}


}
