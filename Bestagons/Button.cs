using System;
using System.Numerics;
using Raylib_cs;

namespace Bestagons;

public class Button
{
    protected string text;
    Color color;
    Color lineColor;
    int lineThickness;
    Rectangle rect;

    protected bool selected = false;
    bool hovered = false;

    Action thing;

    public Button(string text, Color color, Color lineColor, Vector2 position, Vector2 size, Action thing)
    {
        this.text = text;
        this.color = color;
        this.lineColor = lineColor;
        this.thing = thing;
        rect = new Rectangle(position.X, position.Y, size.X, size.Y);
        lineThickness = 2;
    }
    public virtual void Update()
    {
        hovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect);
        if (hovered|| selected)
        {
            lineThickness = 3;
        }
        else
        {
            lineThickness = 2;
        }

        if (hovered&& Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            selected = true;
            if (thing != null)
            {
                thing();
            }
        }
        else if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            selected = false;
        }
    }
    public void Draw()
    {
        Raylib.DrawRectangleRec(rect, color);
        Raylib.DrawRectangleLinesEx(rect, lineThickness, lineColor);
        Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text, 20, 1);
        Vector2 textPos = new Vector2(rect.X + rect.Width / 2 - textSize.X / 2, rect.Y + rect.Height / 2 - textSize.Y / 2);
        Raylib.DrawTextEx(Raylib.GetFontDefault(), text, textPos, 20, 1, Color.Black);
    }
    public void ChangeAction(Action newThing)
    {
        thing = newThing;
    }
}
