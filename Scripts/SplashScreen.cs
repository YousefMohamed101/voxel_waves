using Godot;
using System;

public partial class SplashScreen : ColorRect
{
	private TextureRect logo_A;
	private AnimationPlayer Anim;
	private GameMusicHandler DJ;
	public override void _Ready()
	{
		Anim = GetNode<AnimationPlayer>("CenterContainer/AnimationPlayer");
		GetNode<Timer>("Timer").Timeout += () => GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("MainMenu.tscn");
		Anim.Play("FadeIn");
		DJ = GetNode<GameMusicHandler>("/root/GameMusicHandler");
		DJ.Player.Stream = (AudioStream)GD.Load("res://Music/BG/IntroMusic.mp3");
		DJ.Player.Play();

	}

}
