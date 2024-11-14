using Godot;
using System;

public partial class GameOver : CanvasLayer
{
    private Button Quit;
    private Button Mainemenu;
    private GameDAta _data;
    private int Highscore;
    private Label highscorelabel;

    public override void _Ready()
    {
        RuntimeLoad();
        Mainemenu = GetNode<Button>("/root/World/GameOver/CenterContainer/VBoxContainer/Back_to_main_menu");
        Quit = GetNode<Button>("/root/World/GameOver/CenterContainer/VBoxContainer/Quit_Game");
        highscorelabel = GetNode<Label>("/root/World/GameOver/CenterContainer/VBoxContainer/Highscore");
        highscorelabel.Text = $"HighScore: {_data.Highscore}";
        Quit.Pressed += Quit_Game;
        Mainemenu.Pressed += Back_to_main_menu;
    }
    private void _LoadData(GameDAta data)
    {
        _data = data;


    }
    private void RuntimeLoad()
    {
        string fileName = "res://Resources/GameData.tres";
        if (ResourceLoader.Exists(fileName))
        {

            _LoadData(ResourceLoader.Load<GameDAta>(fileName, null, ResourceLoader.CacheMode.Ignore));
        }
    }

    private void SaveData()
    {
        string fileName = "res://Resources/GameData.tres";
        ResourceSaver.Save(_data, fileName);

    }


    private void Back_to_main_menu()
    {

        _data.highscoreregister();
        SaveData();
        GetNode<SceneTransition>("/root/SceneTransitions").ChangeScene("MainMenu.tscn");
    }
    private void Quit_Game()
    {

        _data.highscoreregister();
        SaveData();
        GetTree().Quit();
    }
}
