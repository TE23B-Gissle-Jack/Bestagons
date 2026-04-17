using System;
using Raylib_cs;
using System.Numerics;

namespace Bestagons;

public class Menu
{
    protected List<Button> buttons = new List<Button>();
    protected Rectangle bigbox;//yes//bg

    protected String title;
    protected Color color = new Color(150, 150, 150, 200);

    public Menu(string title,Rectangle bixbox, string[] btnText, Action[] btnActions)
    {
        this.bigbox = bixbox;
        this.title = title;

        int gap = 30;
        int btnWidth = (int)bixbox.Width - gap*2;

        for (int i = 0; i < btnText.Length; i++)
        {
            //spud hard coded
            buttons.Add(new Button(btnText[i], Color.Gray, Color.Black, new Vector2(bigbox.X + gap, bigbox.Y + 70 + i * 60), new Vector2(btnWidth, 20), btnActions[i]));
        }
    }

    public virtual void Update()
    {
        foreach (Button button in buttons)
        {
            button.Update();
        }
    }
    public virtual void Draw()
    {
        Raylib.DrawRectangleRec(bigbox, color);
        Raylib.DrawRectangleLinesEx(bigbox, 3, Color.Black);
        Vector2 titleSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), title, 30, 1);
        Vector2 titlePos = new Vector2(bigbox.X + bigbox.Width / 2 - titleSize.X / 2, bigbox.Y + 20);
        Raylib.DrawText(title, (int)titlePos.X, (int)titlePos.Y, 30, Color.Black);
        
        foreach (Button button in buttons)
        {
            button.Draw();
        }
    }
}
