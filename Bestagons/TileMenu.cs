using System;
using Raylib_cs;
using System.Numerics;

namespace Bestagons;

public class TileMenu: Menu
{
    int troopCount;
    public TileMenu(string title, Rectangle bigbox, string[] btnText, Action[] btnActions, int troopCount) : base(title, bigbox, btnText, btnActions)
    {
        for (int i = 0; i < btnText.Length; i++)
        {
            //spud hard coded
            //buttons.Add(new Button(btnText[i], Color.Gray, Color.Black, new Vector2(bigbox.X + 30, bigbox.Y + 90 + i * 40), new Vector2((int)bigbox.Width - 60, 20), btnActions[i]));
            buttons.Add(new CountBtn("0", Color.White, Color.Black, new Vector2(bigbox.X + 30, bigbox.Y + 100 + i * 60), new Vector2((int)bigbox.Width - 60, 25), Console.WriteLine));
            this.troopCount = troopCount;
        }
    }
    public override void Draw()
    {
        Raylib.DrawRectangleRec(bigbox, color);
        Raylib.DrawRectangleLinesEx(bigbox, 3, Color.Black);
        
        Vector2 titlePos = pos(10,20, title);
        Raylib.DrawText(title, (int)titlePos.X, (int)titlePos.Y, 20, Color.Black);

        Vector2 textpos = pos(30,20, "Troops: " + troopCount);
        Raylib.DrawText("Troops: " + troopCount, (int)textpos.X, (int)textpos.Y, 20, Color.Black);
        
        foreach (Button button in buttons)
        {
            button.Draw();
        }
    }
    Vector2 pos(int basePos,int size, string text)
    {
        Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text, size, 1);
        return new Vector2(bigbox.X + bigbox.Width / 2 - textSize.X / 2, bigbox.Y + basePos);
    }
}
