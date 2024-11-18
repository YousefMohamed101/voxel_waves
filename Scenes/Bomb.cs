using Godot;
using System;
using System.Collections.Generic;

public partial class Bomb : RigidBody3D
{

    public int Damage = 25;
    public Area3D BlastRadius;
    public Timer Counter;
    [Export] public PackedScene explosinonVFX;
    private HashSet<Node> damagedEnemies = new HashSet<Node>();

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
                Node targetEnemy;

                // Determine the actual enemy node
                if (body is Zombie1)
                {
                    targetEnemy = body;
                }
                else
                {
                    targetEnemy = body.GetParent().GetParent().GetParent().GetParent();
                }
                if (damagedEnemies.Contains(targetEnemy))
                    continue;

                GD.Print("damaged");
                var source = this.GlobalTransform.Origin;

                // Add the enemy to our tracked list before dealing damage
                damagedEnemies.Add(targetEnemy);

                // Apply damage to the appropriate target
                if (body is Zombie1)
                {
                    targetEnemy.Call("recieve_damage", Damage, 300, source);
                }
                else
                {
                    targetEnemy.Call("recieve_damage", Damage, 300, source);
                }
            }
            QueueFree();
        }

    }
}
