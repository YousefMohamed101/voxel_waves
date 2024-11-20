using Godot;
using System;
using System.Collections.Generic;


public partial class ItemSpawner : Node3D
{
	// Define your items and their weights here
	private List<(PackedScene Scene, int Weight)> _items = new List<(PackedScene, int)>
	{
		(ResourceLoader.Load<PackedScene>("res://3d models/Guns/GLockGunPick.tscn"), 10),
		(ResourceLoader.Load<PackedScene>("res://3d models/Guns/AK47Pick.tscn"), 10),
		(ResourceLoader.Load<PackedScene>("res://3d models/props/ammobox.tscn"), 15),
	};

	[Export]
	public Vector3 SpawnAreaSize { get; set; } = new Vector3(50, 2, 50);

	[Export]
	public int NumItemsToSpawn { get; set; } = 5;

	private RandomNumberGenerator _rng = new RandomNumberGenerator();
	private Timer TISpawn;

	public override void _Ready()
	{
		TISpawn = GetNode<Timer>("ITT");
		TISpawn.Timeout += () => SpawnItems();

	}

	public void SpawnItems()
	{
		int totalWeight = 0;
		foreach (var item in _items)
		{
			totalWeight += item.Weight;
		}

		for (int i = 0; i < NumItemsToSpawn; i++)
		{
			float randomWeight = _rng.RandfRange(0, totalWeight);
			PackedScene selectedScene = null;

			foreach (var item in _items)
			{
				randomWeight -= item.Weight;
				if (randomWeight <= 0)
				{
					selectedScene = item.Scene;
					break;
				}
			}

			if (selectedScene != null)
			{
				Node3D instance = selectedScene.Instantiate<Node3D>();
				AddChild(instance);

				// Set random position within spawn area
				Vector3 randomPosition = new Vector3(
					_rng.RandfRange(-SpawnAreaSize.X / 2, SpawnAreaSize.X / 2),
					SpawnAreaSize.Y,
					_rng.RandfRange(-SpawnAreaSize.Z / 2, SpawnAreaSize.Z / 2)
				);
				instance.GlobalTransform = new Transform3D(Basis.Identity, randomPosition);
			}
		}
		GD.Print("ItemSpawned");
		TISpawn.Start();
	}


}
