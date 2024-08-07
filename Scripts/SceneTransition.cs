using Godot;
using System;

public partial class SceneTransition : Node
{
	public void ChangeScene(string name)
	{
		GetTree().ChangeSceneToFile($"res://Scenes/{name}");
	}
}
