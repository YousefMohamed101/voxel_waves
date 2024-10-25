using Godot;
using System;

public partial class Weapon : Node3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public string WeaponName { get; set; } = "Default Weapon";
	[Export] public int Damage { get; set; } = 10;
	[Export] public int MagazineClip { get; set; } = 20;


	[Signal]
	public delegate void MagazineChangeEventHandler(int clipCount, int magazineCount);



	public void EmitMagazineChange(int clipCount, int magazineCount)
	{
		EmitSignal(nameof(MagazineChange), clipCount, magazineCount);
	}
	public virtual void UpdateMagazine()
	{

	}


}
