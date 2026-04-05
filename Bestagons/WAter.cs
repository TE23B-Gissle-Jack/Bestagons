using System;
using System.Numerics;
using Raylib_cs;

namespace Bestagons;

public class WAter : Tile
{
    public WAter(Vector2 position) : base(position)
    {
        baseColor = Color.SkyBlue;
        fillColor = baseColor;
    }
    public override void Draw()
    {
        DrawFilling(baseColor);
        DrawOutline(lineColor);
        DrawTroops([textColor,fillColor],[15,5], 50);

    }
    // public override void changeOwner(Player target)
    // {
    //     owner = target;//wow
    // }
}
