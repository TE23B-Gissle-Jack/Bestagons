using System;
using System.Numerics;
using Raylib_cs;

namespace Bestagons;

public class Tile
{
    Vector2 orgin;
    static List<Vector2> tilePositions = new List<Vector2>();
    List<Vector2> corners = new List<Vector2>();
    //0 = k; 1 = m;
    List<float[]> functions = new List<float[]>();
    List<float> functionRelastion = new List<float>();

    public Tile(Vector2 position)
    {
        orgin = position;
        tilePositions.Add(position);
    }


    public void Draw()
    {
        Raylib.DrawCircleV(orgin, 5, Color.Red);
        foreach (var corner in corners)
        {
            Raylib.DrawCircleV(corner, 3, Color.Purple);
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

                Raylib.DrawLineV(new(0, k2 * 0 + m), new(Raylib.GetScreenWidth(), k2 * Raylib.GetScreenWidth() + m), Color.Green);
                functions.Add([k2, m]);
                Console.WriteLine(m);
            }
        }
        functions.Add([0, Raylib.GetScreenHeight()]);
        functions.Add([0, 0]);
        functions.Add([int.MaxValue, Raylib.GetScreenHeight()]);
        functions.Add([int.MaxValue, 0]);
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
                    corners.Add(new(x, x * yeep[0] + yeep[1]));
                }
            }
        }
        CompareToLine(0,orgin,true);
        //Chack if Corners are on the "wrong" side of lines
        for (int i = corners.Count - 1; i >= 0; i--)
        {
            CompareToLine(i, corners[i]);
        }
    }
    //why take vector2 and index?.... hmmmmm..... lazy
    void CompareToLine(int function, Vector2 point, bool orgin = false)
    {
        for (int i = 0; i < functions.Count; i++)
        {
            //d=(x−x1)(y2−y1)−(y−y1)(x2−x1)
            //x1,y1 && x2,y2 = line
            //x,y = point to of intrest
            Vector2 lineStart = new(0, functions[i][0] * 0 + functions[i][1]);
            Vector2 lineEnd = new(Raylib.GetScreenWidth(), functions[i][0] * Raylib.GetScreenWidth() + functions[i][1]);
            float d = (point.X - lineStart.X) * (lineEnd.Y - lineStart.Y) - (point.Y - lineStart.Y) * (lineEnd.X - lineStart.X);
            //If d<0 then the point lies on one side of the line, and if d>0 then it lies on the other side. If d=0 then the point lies exactly line.
            if (orgin)
            {
                functionRelastion.Add(d);
            }
            else
            {
                //bool less = functionRelastion[i]<0;
                if (functionRelastion[i] * d < 0)
                {
                    corners.Remove(point);
                    Console.WriteLine("DIe");
                    //functionRelastion.RemoveAt(i);
                    break;
                }
            }
        }

    }
}
