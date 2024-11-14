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
	private Random cellR = new Random();
	private GridMap WorldMap;
	private Timer EnSpawn;
	private int waveno = 5;

	public override void _Ready()
	{
		EnSpawn = GetNode<Timer>("EnemySpawnTime");
		WorldMap = GetNode<GridMap>("/root/World/Land");
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
		var cells = WorldMap.GetUsedCells();
		NumEnemiesToSpawn = x;
		int totalWeight = 0;
		foreach (var item in _items)
		{
			totalWeight += item.Weight;
		}

		for (int i = 0; i < NumEnemiesToSpawn; i++)
		{
			if (cells.Count == 0)
				break;
			float randomWeight = _rng.RandfRange(0, totalWeight);
			PackedScene selectedScene = null;
			int randomcell = cellR.Next(cells.Count);

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
				Vector3I cellPos = cells[randomcell];
				Vector3 worldPos = WorldMap.MapToLocal(cellPos);
				Node3D instance = selectedScene.Instantiate<Node3D>();
				AddChild(instance);

				// Set random position within spawn area
				Vector3 randomPosition = worldPos;
				instance.GlobalTransform = new Transform3D(Basis.Identity, randomPosition);
			}

		}
		waveno += 5;

	}


}


