using Godot;
using System;

public partial class GameMusicHandler : Node
{
	public AudioStreamPlayer Player;
	public override void _Ready()
	{
		Player = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
	}
}
