using Godot;
using System;

public partial class Bomb : RigidBody3D
{
    public bool triggered = false;
    public int Damage = 25;
    public Area3D BlastRadius;
    public Timer Counter;
    public override void _Ready()
    {
        BlastRadius = GetNode<Area3D>("Blast");
        Counter = GetNode<Timer>("Timer");
    }
    public void throwGrenade()
    {
        if (triggered == false)
        {
            Counter.Start();
            triggered = true;
        }
    }
    public void timertimeout()
    {
        var bodies = BlastRadius.GetOverlappingBodies();
        foreach (var body in bodies)
        {
            if (body is Node node && node.IsInGroup("Zombies"))
            {
                var space = GetWorld3D().DirectSpaceState;
                var collision = PhysicsRayQueryParameters3D.Create(GlobalTransform.Origin, body.GlobalTransform.Origin, CollisionMask, new Godot.Collections.Array<Rid> { GetRid() });
                var result = space.IntersectRay(collision);
                if (body.HasMethod("recieve_damage")) // Ensure the body has a health property
                {
                    body.Call("recieve_damage", Damage);
                }
            }
        }
        QueueFree();
    }
}
