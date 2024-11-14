using Godot;
using System;

public partial class Explosionparticle : Node3D
{
    public AnimationPlayer Anime;

    public override void _Ready()
    {
        Anime = GetNode<AnimationPlayer>("AnimationPlayer");
        Anime.Play("BOOM");
        Anime.AnimationFinished += OnAnimationFinished;
    }
    public void OnAnimationFinished(StringName anim_name){
         if (anim_name == "BOOM")
		{
			 QueueFree();
		}
    }
}
