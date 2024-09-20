using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node3D
{
	// Called when the node enters the scene tree for the first time.

	// Define your items and their weights here
	private List<(PackedScene Scene, int Weight)> _items = new List<(PackedScene, int)>
	{
		(ResourceLoader.Load<PackedScene>("res://Zombie1.tscn"), 10),

	};

	[Export]
	public Vector3 SpawnAreaSize { get; set; } = new Vector3(100, 2, 100);

	[Export]
	public int NumEnemiesToSpawn { get; set; } = 5;

	private RandomNumberGenerator _rng = new RandomNumberGenerator();
	private Timer EnSpawn;
	private int waveno = 5;

	public override void _Ready()
	{
		EnSpawn = GetNode<Timer>("EnemySpawnTime");
		WaveSpawn();
		EnSpawn.Timeout += WaveSpawn;

	}
	private void WaveSpawn()
	{
		SpawnEnemies(waveno);
		waveno += 5;
		GD.Print("spawned");
		EnSpawn.Start();
	}

	public void SpawnEnemies(int x)
	{
		NumEnemiesToSpawn = x;
		int totalWeight = 0;
		foreach (var item in _items)
		{
			totalWeight += item.Weight;
		}

		for (int i = 0; i < NumEnemiesToSpawn; i++)
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


	}


}


