using System;
using System.Numerics;
using Raylib_cs;

namespace Bestagons;

public class Land : Tile
{
    static List<Tile> tiles = new List<Tile>();

    public static bool hideLines = false;//for testing
    bool hovered = false;
    bool selected = false;

    protected Color baseColor = Color.Gray;
    protected Color fillColor = new(50 + Random.Shared.Next(195), 50 + Random.Shared.Next(195), 50 + Random.Shared.Next(195));
    protected Color lineColor = Color.Black;
    protected Color textColor = Color.Black;

    int lineThicknes = 2;
    int lineHoverThicknes = 4;
    List<Vector2> baseLineCorners = new List<Vector2>();
    List<Vector2> hoverLineCorners = new List<Vector2>();

    protected int trops;// = 0;
    public bool attacking { get; set; }
    int attackingTroops = 0;
    public int Troops
    {
        get
        {
            return attackingTroops;
        }
        set
        {
            attackingTroops = Math.Min(trops - 1, value);
        }
    }
    protected Player owner = null;

    static bool mouseDoingOherShit = false;

    TileMenu test;

    public Land(Vector2 position) : base(position)
    {
        tiles.Add(this);
        trops = Random.Shared.Next(1, 10);
        test = new TileMenu("Owner: " + owner?.name, new Rectangle(100, 100, 200, 300), new string[] { "Attack", "Move Troops" }, new Action[] { () => Console.WriteLine("Button1 clicked"), () => Console.WriteLine("Button2 clicked") }, 2, trops, this);
    }

    public virtual void Draw()
    {
        //Raylib.DrawCircleV(orgin, 5, Color.Red);
        DrawFilling(fillColor);
        //DrawTroops([textColor],[5], 20);
    }
    public void Draw2()//layering bullshit
    {
        if (baseLineCorners.Count ==0 )
        {
            baseLineCorners = OfsetEdge(lineHoverThicknes/2);
            hoverLineCorners = OfsetEdge(lineHoverThicknes/2);
        }

        DrawOutline(lineColor);
        if (attacking)
        {
            Vector2 mouse = Raylib.GetMousePosition();

            Vector2 direction = center - mouse;

            float angle = MathF.Atan2(direction.Y, direction.X);

            List<Vector2> triangle = [new Vector2(10, 0), new Vector2(50, 20), new Vector2(50, -20)];
            List<Vector2> real = new List<Vector2>();
            for (int i = 0; i < triangle.Count; i++)
            {
                Vector2 point = triangle[i];
                real.Add(new Vector2(
                    mouse.X + point.X * MathF.Cos(angle) - point.Y * MathF.Sin(angle),
                    mouse.Y + point.Y * MathF.Cos(angle) + point.X * MathF.Sin(angle)
                //         this.position.x + x * cos - y * sin,
                //         this.position.y + y * cos + x * sin
                ));
            }
            Raylib.DrawLineEx(center, real[0], 10, Color.Black);
            Raylib.DrawTriangle(mouse, real[1], real[2], Color.Black);
        }
    }
    public void Draw3()//should probobly nort extist
    {
        if (selected && !attacking)
        {
            test.Draw();
        }
    }
    protected void DrawFilling(Color color)
    {
        //filling
        if (corners.Count > 2)
        {
            Vector2[] fan = new Vector2[corners.Count + 1];
            fan[0] = center;

            for (int i = 0; i < corners.Count; i++)
            {
                fan[i + 1] = corners[i];
            }

            for (int i = 1; i < corners.Count; i++)
            {
                Raylib.DrawTriangle(center, corners[i], corners[i - 1], color);
            }
            Raylib.DrawTriangle(center, corners[0], corners[corners.Count - 1], color);
        }
    }
    protected void DrawOutline(Color color)
    {
        //outline
        if (baseLineCorners.Count > 1)
        {
            int thicknes = lineThicknes;
            List<Vector2> defining = baseLineCorners;
            if (hovered || selected)
            {
                thicknes = lineHoverThicknes;
                defining = hoverLineCorners;
            }
            //Raylib.DrawLineV(corners[0], corners[corners.Count - 1], lineColor);
            Vector2[] yeah = new Vector2[defining.Count + 1];
            for (int i = 0; i < defining.Count; i++)
            {
                yeah[i] = defining[i];
            }
            yeah[defining.Count] = defining[0];

            Raylib.DrawSplineLinear(yeah, yeah.Length, thicknes, color);
        }
    }
    protected virtual void DrawTroops(Color[] colors, int[] thickness, int textSize)
    {
        string text = "" + trops;
        int k = 0; //i dont want to think
        for (int i = 0; i < colors.Length; i++)
        {
            for (int j = 0; j < thickness[i]; j++)
            {
                Raylib.DrawText(text, (int)center.X + k / 3, (int)center.Y + k / 2, textSize - i, colors[i]);
                if (textSize - k > 1) k++;
            }
        }
    }
    public bool defend(int attaking)
    {
        while (attaking > 0 && trops > 0)
        {
            int attacker = Random.Shared.Next(10);
            int defender = Random.Shared.Next(10);
            if (defender >= attacker)
            {
                attaking -= 1;
            }
            else trops -= 1;
        }
        if (trops > 0)
        {
            test = new TileMenu("Owner: " + owner?.name, new Rectangle(100, 100, 200, 300), new string[] { "Attack", "Move Troops" }, new Action[] { () => Console.WriteLine("Button1 clicked"), () => Console.WriteLine("Button2 clicked") }, 2, trops, this);
            return true;
        }
        else
        {
            test = new TileMenu("Owner: " + owner?.name, new Rectangle(100, 100, 200, 300), new string[] { "Attack", "Move Troops" }, new Action[] { () => Console.WriteLine("Button1 clicked"), () => Console.WriteLine("Button2 clicked") }, 2, trops, this);
            trops = attaking;
            return false;
        }
    }
    bool attack(Land target, int amt)
    {
        Console.WriteLine("Attack! " + amt);
        trops -= amt;
        attacking = false;
        selected = false;
        mouseDoingOherShit = false;
        if (!target.defend(amt))
        {
            target.changeOwner(owner);
            //stupid
            test = new TileMenu("Owner: " + owner?.name, new Rectangle(100, 100, 200, 300), new string[] { "Attack", "Move Troops" }, new Action[] { () => Console.WriteLine("Button1 clicked"), () => Console.WriteLine("Button2 clicked") }, 2, trops, this);
            return true;
        }
        //stupid
        test = new TileMenu("Owner: " + owner?.name, new Rectangle(100, 100, 200, 300), new string[] { "Attack", "Move Troops" }, new Action[] { () => Console.WriteLine("Button1 clicked"), () => Console.WriteLine("Button2 clicked") }, 2, trops, this);
        return false;
    }   //  TANGENT
    bool CheckTarget(Land target)
    {
        if (target == null) return true;
        else if (target == this) return false;
        else if (target.owner == owner) return true;
        else if (target.owner == null) return true;
        else return false;//idk
    }
    public virtual void changeOwner(Player target)
    {
        owner = target;
        if (target != null) fillColor = target.color;
        else fillColor = baseColor;
        //stupid
        test = new TileMenu("Owner: " + owner?.name, new Rectangle(100, 100, 200, 300), new string[] { "Attack", "Move Troops" }, new Action[] { () => Console.WriteLine("Button1 clicked"), () => Console.WriteLine("Button2 clicked") }, 2, trops, this);
    }
    public void Update()
    {
        //    bool CheckCollisionPointPoly(Vector2 point, const Vector2 *points, int pointCount);                // Check if point is within a polygon described by array of vertices
        hovered = Raylib.CheckCollisionPointPoly(Raylib.GetMousePosition(), corners.ToArray());                      //Raylib.CheckCollisionPointCircle(Raylib.GetMousePosition(), orgin, 5);
        if (hovered)
        {
            //hideLines = true;
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && !mouseDoingOherShit)
            {
                selected = true;
                mouseDoingOherShit = true;
            }
        }
        //defacto means something else is hoverd and then selected
        //will not work that way in a sec
        else if (Raylib.IsMouseButtonPressed(MouseButton.Right))
        {
            selected = false;
            mouseDoingOherShit = false;
            attacking = false;
        }
        if (selected)
        {
            test.Update();
        }
        if (attacking)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                foreach (Land tile in tiles)
                {
                    if (tile.hovered)
                    {
                        if (CheckTarget(tile)) continue;
                        attack(tile, attackingTroops);
                    }
                }
            }
        }
    }

    List<Vector2> OfsetEdge(int amt)
    {
        List<Vector2> yes = new List<Vector2>();
        foreach (var corner in corners)
        {
            Vector2 direction = center - corner;
            yes.Add(corner + direction / direction.Length() * amt / 2);
        }
        return yes;
    }
}
