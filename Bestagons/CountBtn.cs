using System;
using Raylib_cs;
using System.Numerics;

namespace Bestagons;

public class CountBtn : Button
{
    int count;
    public int Amt
    {
        get { return count; }
        private set { count = value; }
    }

    //idk how else to do it... is there another way?
    public CountBtn(string text, Color color, Color lineColor, Vector2 position, Vector2 size, Action thing) : base(text, color, lineColor, position, size, thing)
    {

    }

    public override void Update()
    {
        base.Update();
        if (selected)
        {
            int key = Raylib.GetKeyPressed();
            // 48 - 0
            // 49 - 1
            // 50 - 2
            // 51 - 3
            // 52 - 4
            // 53 - 5
            // 54 - 6
            // 55 - 7
            // 56 - 8
            // 57 - 9
            //8 - Backspace ///LIES! 259!!!!
            int[] valid = { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 259 };
            if (valid.Contains(key))
            {
                if (key == 259)
                {
                    if (text.Length > 1)
                    {
                        text = text.Substring(0, text.Length - 1);
                    }
                    else text = "0";
                }
                else
                {
                    if (text == "0") text = "";
                    text += (key - 48).ToString();
                }
                //should not fail so who cares.. not my problem
                int.TryParse(text, out count);
            }
            Console.WriteLine(key);
        }
    }
}
