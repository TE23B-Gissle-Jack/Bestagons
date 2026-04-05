using System;
using Raylib_cs;

namespace Bestagons;

public class Player
{
    string name;
    public Color color;
    public Player(string name, Color color)
    {
        this.name = name;
        this.color = color; 
    }
}
