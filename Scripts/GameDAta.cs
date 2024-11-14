using Godot;
using System;

[GlobalClass]
public partial class GameDAta : Resource
{
    [Export] public int Highscore = 0;
    [Export] public int Score = 0;
    [Export] public bool deads = false;
    public void highscoreregister()
    {
        if (Highscore < Score)
        {
            Highscore = Score;
        }
    }

}
