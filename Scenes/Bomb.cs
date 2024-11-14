using Godot;
using System;

public partial class Bomb : RigidBody3D
{

    public int Damage = 25;
    public Area3D BlastRadius;
    public Timer Counter;
    [Export] public PackedScene explosinonVFX;

    public override void _Ready()
    {

        BlastRadius = GetNode<Area3D>("Blast");
        Counter = GetNode<Timer>("Timer");
        Counter.Start();
        Counter.Timeout += timertimeout;

    }
    public void timertimeout()
    {
        var bodies = BlastRadius.GetOverlappingBodies();
        Node3D VFX = explosinonVFX.Instantiate<Node3D>();
        VFX.Position = this.GlobalPosition;
        GetTree().Root.AddChild(VFX);
        foreach (var body in bodies)
        {
            if (body.IsInGroup("Zombies"))
            {
                var par = body.GetParent().GetParent().GetParent().GetParent();
                var source = this.GlobalTransform.Origin;

                par.Call("recieve_damage", Damage, 1000, source);
                GD.Print("damaged");
            }
        }
        QueueFree();
    }

}
