using System.Numerics;
using Bestagons;
using Raylib_cs;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


Raylib.InitWindow(1400, 700, "Kraka Kapaw");
Raylib.SetTargetFPS(60);
Raylib.SetExitKey(KeyboardKey.Null);

List<Tile> bᛟᚨᛏ = new List<Tile>();
for (int x = 0; x < Raylib.GetScreenWidth(); x++)
{
    for (int y = 0; y < Raylib.GetScreenHeight(); y++)
    {
        if (Random.Shared.Next(32000) == 50)
        {
            bᛟᚨᛏ.Add(new Tile(new(x, y)));
        }
    }
}
bᛟᚨᛏ[0].Define();
Raylib.BeginDrawing();

foreach (var item in bᛟᚨᛏ)
{
    item.Draw();
}

// foreach (Vector2 point in bᛟᚨᛏ)
// {
//     for (int i = 0; i < bᛟᚨᛏ.Count; i++)
//     {
//         Vector2 victor = bᛟᚨᛏ[i];
//         if (victor!=point)
//         {
//             Raylib.DrawLineV(victor,point,Color.Blue);
//             Vector2 middle = new(point.X+(victor.X-point.X)/2,point.Y+(victor.Y-point.Y)/2);
//             Raylib.DrawCircleV(middle,2,Color.Gold);
//             float k = (victor.Y-point.Y)/(victor.X-point.X);
//             float k2 = -1/k;
//             Vector2 Woff(float x)
//             {
//                 //y=kx+m    m=kx-y
//                 float m = k2*middle.X-middle.Y;
//                 return new(x,k2*x+m);
//             }
//             Raylib.DrawLineV(Woff(0),Woff(Raylib.GetScreenWidth()),Color.Green);
//         }
//     }
// }

// Vector2 orgin = bᛟᚨᛏ[0];
// for (int i = 0; i < bᛟᚨᛏ.Count; i++)
// {
//     Vector2 victor = bᛟᚨᛏ[i];
//     if (victor != orgin)
//     {
//         Raylib.DrawLineV(victor, orgin, Color.Blue);
//         Vector2 middle = new(orgin.X + (victor.X - orgin.X) / 2, orgin.Y + (victor.Y - orgin.Y) / 2);
//         Raylib.DrawCircleV(middle, 2, Color.Gold);
//         float k = (victor.Y - orgin.Y) / (victor.X - orgin.X);
//         float k2 = -1 / k;
//         Vector2 Woff(float x)
//         {
//             //y=kx+m    m=kx-y
//             float m = middle.Y - k2 * middle.X;
//             Console.WriteLine(x + " " + (k2 * x + m));
//             return new(x, k2 * x + m);
//         }
//         Raylib.DrawLineV(Woff(0), Woff(Raylib.GetScreenWidth()), Color.Green);
//     }
//     Raylib.DrawCircleV(bᛟᚨᛏ[i],4,Color.Red);
// }


Raylib.EndDrawing();

Console.ReadLine();