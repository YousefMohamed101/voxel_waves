using Godot;
using System;

public partial class Unpause2 : CanvasLayer
{
    private CenterContainer InGameMenu;
    private ColorRect MenuShade;
    private GameDAta _data;
    private bool dead;
    public GameMusicHandler DJ;
    public override void _Ready()
    {
        InGameMenu = GetNode<CenterContainer>("CenterContainer");
        MenuShade = GetNode<ColorRect>("ColorRect");
        dead = false;
        DJ = GetNode<GameMusicHandler>("/root/GameMusicHandler");

    }
    private void _LoadData(GameDAta data)
    {
        _data = data;
        dead = _data.deads;
    }
    private void RuntimeLoad()
    {
        string fileName = "res://Resources/GameData.tres";
        if (ResourceLoader.Exists(fileName))
        {

            _LoadData(ResourceLoader.Load<GameDAta>(fileName, null, ResourceLoader.CacheMode.Ignore));
        }

    }

    public override void _Input(InputEvent @event)
    {
        RuntimeLoad();
        if (@event is InputEventKey e && e.IsPressed() && e.Keycode == Key.Escape && DJ.deadss == false)
        {
            GetViewport().SetInputAsHandled();
            if (GetTree().Paused)
            {
                GetTree().Paused = false;
                InGameMenu.Visible = false;
                MenuShade.Visible = false;
                DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
            }
        }
    }
}
