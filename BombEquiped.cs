using Godot;
using System;

public partial class BombEquiped : Weapon
{
    public bool triggered = false;
    public new int Damage = 25;
    public Area3D BlastRadius;
    public Timer Counter;
    public Camera3D camera;
    private Timer Reloadtime;
    public MeshInstance3D BombModel;
    [Export] public PackedScene model;
    private bool mouse_left_down;
    private AnimationPlayer Anime;
	private bool Walking;
    [Export] public int MagazineSize { get; set; } = 30;
	[Export] public int clips { get; set; } = 10;
    private string currentAnimation = "IDLE";
    private bool reloading = false; // Track reload state


    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
            {
                mouse_left_down = true;
            }
            else
            {
                mouse_left_down = false;
            }
        }
    }

    public void refill()
	{
		clips = 10;
		EmitMagazineChange(clips, MagazineSize);
	}



    public override void _Ready()
    {
        camera = GetViewport().GetCamera3D();
        Counter = GetNode<Timer>("FireRate");
        BombModel = GetNode<MeshInstance3D>("Cylinder_025");
        Counter.Timeout += Timershoot;
        Anime = GetNode<AnimationPlayer>("AnimationPlayer");
        Reloadtime = GetNode<Timer>("ReloadTime");
		Reloadtime.Stop();
        EmitMagazineChange(clips, MagazineSize);
        
    }
    public void throwGrenade()
    {
        if (triggered == false && camera != null && model != null)
        {

            var camera_transform = camera.GlobalTransform;

            var forward = -camera_transform.Basis.Z.Normalized();
            var grenade = model.Instantiate<RigidBody3D>();
            grenade.Position = camera_transform.Origin + (forward * 0.5f);
            //grenade.LinearVelocity = forward * 10.0f;
            grenade.ApplyCentralImpulse(forward * 20 + new Vector3(0, 3.5f, 0));
            GetTree().Root.AddChild(grenade);
            triggered = true;
            BombModel.Visible = false;
            
            EmitMagazineChange(clips, MagazineSize);
        }
        else if (camera == null)
        {
            GD.PrintErr("Camera is not set or could not be retrieved.");
        }
        else if (model == null)
        {
            GD.PrintErr("Model (PackedScene) is not assigned.");
        }
    }
    public void Timershoot()
    {
        BombModel.Visible = true;
        triggered = false;
        GD.Print("done");
    }
   
    public override void _PhysicsProcess(double delta)
	{
		Walking = Input.IsActionPressed("Forward") || Input.IsActionPressed("Backward") || Input.IsActionPressed("Right") || Input.IsActionPressed("Left");



		if (mouse_left_down && Counter.IsStopped() && Reloadtime.IsStopped() )
		{
            Anime.Stop();
			throwGrenade();
			MagazineSize -= 1;
			EmitMagazineChange(clips, MagazineSize);
            if (MagazineSize == 0 || !Counter.IsStopped())
			{
				mouse_left_down = false;

			}
            GD.Print("Shooting");
            Counter.Start();
		}
		else if (Input.IsActionPressed("Reloading") && Reloadtime.IsStopped())
        {
            
            clips -= 1;
            MagazineSize = 30;
            EmitMagazineChange(clips, MagazineSize);
            Reloadtime.Start(); 
            PlayAnimation("Reload");
            GD.Print("Reloaded");
        }
		else if (!mouse_left_down && Reloadtime.IsStopped())
		{
			if (Walking)
			{
				PlayAnimation("Walking");
			}
			else
			{
				PlayAnimation("IDLE");
			}
			
		}

	}
    private void PlayAnimation(string animationName)
	{
		if (currentAnimation == animationName) return;
		Anime.Play(animationName);
		currentAnimation = animationName;
	}
}
