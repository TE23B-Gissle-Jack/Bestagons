using System;
using System.Numerics;
using Raylib_cs;

namespace Bestagons;

public class Tile
{
    Vector2 orgin;
    static List<Vector2> tilePositions = new List<Vector2>();
    List<Vector2> Corners = new List<Vector2>();
    //0 = k; 1 = m;
    List<float[]> functions = new List<float[]>();

    public Tile(Vector2 position)
    {
        orgin = position;
        tilePositions.Add(position);
    }


    public void Draw()
    {
        Raylib.DrawCircleV(orgin,5,Color.Red);
        foreach (var corner in Corners)
        {
            Raylib.DrawCircleV(corner,3,Color.Purple);
        }
    }

    void Compare()
    {
        for (int i = 0; i < tilePositions.Count; i++)
        {
            Vector2 victor = tilePositions[i];
            if (victor != orgin)
            {
                //Raylib.DrawLineV(victor, orgin, Color.Blue);
                Vector2 middle = new(orgin.X + (victor.X - orgin.X) / 2, orgin.Y + (victor.Y - orgin.Y) / 2);
                //Raylib.DrawCircleV(middle, 2, Color.Gold);
                float k = (victor.Y - orgin.Y) / (victor.X - orgin.X);
                float k2 = -1 / k;
                    //y=kx+m    m=kx-y
                    float m = middle.Y - k2 * middle.X;

                //Raylib.DrawLineV(new(0, k2 * 0 + m), new(Raylib.GetScreenWidth(), k2 * Raylib.GetScreenWidth() + m), Color.Green);
                functions.Add([k2,m]);
                Console.WriteLine(m);
            }
        }
    }

    public void Define()
    {
        Compare();
        foreach (float[] yeep in functions)
        {
            foreach (float[] xeep in functions)
            {
                if (yeep != xeep)
                {
                    //x*k1+m1 = x*k2+m2
                    float x = (yeep[1] - xeep[1]) / (xeep[0] - yeep[0]);
                    Corners.Add(new(x, x * yeep[0]+yeep[1]));
                }
            }
        }
    }
}
