using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;

namespace Bestagons
{
    public class Tile
    {
        Vector2 orgin;
        static List<Vector2> tilePositions = new List<Vector2>();
        List<Vector2> corners = new List<Vector2>();
        List<Vector2> deadCorners = new List<Vector2>();

        List<float[]> functions = new List<float[]>(); // [A, B, C]
        List<float> functionRelation = new List<float>();
        public static bool hideLines = false;
        bool hovered = false;
        Color lineColor = new(122 + Random.Shared.Next(122), 122 + Random.Shared.Next(122), 122 + Random.Shared.Next(122));

        bool fortniteOG = false;
        List<Vector2> polygon;

        public Tile(Vector2 position)
        {
            orgin = position;
            tilePositions.Add(position);
            this.polygon = new(){
                    new Vector2(0, 0),
                    new Vector2(Raylib.GetScreenWidth(), 0),
                    new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()),
                    new Vector2(0, Raylib.GetScreenHeight())
                };
        }

        public void Draw()
        {
            //Raylib.DrawCircleV(orgin, 5, Color.Red);
            if (!hideLines || hovered)
            {
                if (corners.Count > 1)
                {
                    Raylib.DrawLineV(corners[0], corners[corners.Count - 1], lineColor);
                }

                for (int i = 1; i < corners.Count; i++)
                {
                    Raylib.DrawLineV(corners[i], corners[i - 1], lineColor);
                }
                if (corners.Count > 2)
                {
                    Vector2 center = Vector2.Zero;
                    foreach (var c in corners) center += c;

                    center /= corners.Count;

                    Vector2[] fan = new Vector2[corners.Count + 1];
                    fan[0] = center;

                    for (int i = 0; i < corners.Count; i++)
                    {
                        fan[i + 1] = corners[i];
                    }

                    Raylib.DrawTriangleFan(fan, fan.Length, lineColor);

                    Raylib.DrawTriangleFan(corners.ToArray(), corners.Count, lineColor);
                }
                foreach (var item in corners)
                {
                    //Raylib.DrawCircleV(item, 5, Color.Pink);
                }
                if (hovered)
                {
                    foreach (Vector2 item in deadCorners)
                    {
                        Raylib.DrawCircleV(item, 5, Color.DarkBlue);
                    }
                }
            }

        }
        public void Update()
        {
            hovered = Raylib.CheckCollisionPointCircle(Raylib.GetMousePosition(), orgin, 5);
            if (hovered)
            {
                hideLines = true;
            }
        }

        void Compare()
        {
            foreach (var victor in tilePositions)
            {
                if (Vector2.Distance(victor, orgin) < 0.0001f) continue;

                Vector2 middle = (orgin + victor) / 2f;
                float dx = victor.X - orgin.X;
                float dy = victor.Y - orgin.Y;

                float A = dx;
                float B = dy;
                float C = -(A * middle.X + B * middle.Y);

                functions.Add([A, B, C]);

                // draw the line
                if (B != 0)
                    Raylib.DrawLineV(new Vector2(0, (-C - A * 0) / B), new Vector2(Raylib.GetScreenWidth(), (-C - A * Raylib.GetScreenWidth()) / B), new(0, 255, 0, 50));
                else
                    Raylib.DrawLineV(new Vector2(-C / A, 0), new Vector2(-C / A, Raylib.GetScreenHeight()), new(0, 255, 0, 50));
            }

            // Add screen boundaries
            functions.Add([0, 1, -5]);
            functions.Add([0, 1, -(Raylib.GetScreenHeight() - 5)]);
            functions.Add([1, 0, 0]);
            functions.Add([1, 0, -Raylib.GetScreenWidth()]);
        }

        public void Define()
        {
            if (fortniteOG)
            {

                functions.Clear();
                functionRelation.Clear();
                corners.Clear();

                Compare();

                // Calculate intersections
                for (int i = 0; i < functions.Count; i++)
                {
                    var line1 = functions[i];
                    for (int j = i + 1; j < functions.Count; j++)
                    {
                        var line2 = functions[j];

                        float A1 = line1[0], B1 = line1[1], C1 = line1[2];
                        float A2 = line2[0], B2 = line2[1], C2 = line2[2];

                        float det = A1 * B2 - A2 * B1;

                        if (Math.Abs(det) > 0.0001f) // Not parallel
                        {
                            float x = (B1 * C2 - B2 * C1) / det;
                            float y = (A2 * C1 - A1 * C2) / det;

                            corners.Add(new Vector2(x, y));
                        }
                    }
                }

                // Determine which side of each line the origin is on
                CompareToLine(0, orgin, true);

                // Remove corners outside the polygon
                for (int i = corners.Count - 1; i >= 0; i--)
                {
                    CompareToLine(i, corners[i]);
                }
                SortCornersClockwise();
            }
            else
            {
                functions.Clear();
                corners.Clear();

                foreach (var victor in tilePositions)
                {
                    if (Vector2.Distance(victor, orgin) < 0.0001f) continue;

                    Vector2 middle = (orgin + victor) / 2f;
                    float dx = victor.X - orgin.X;
                    float dy = victor.Y - orgin.Y;

                    float A = dx;
                    float B = dy;
                    float C = -(A * middle.X + B * middle.Y);

                    // orgin inside
                    if (A * orgin.X + B * orgin.Y + C < 0)
                    {
                        A *= -1;
                        B *= -1;
                        C *= -1;
                    }

                    functions.Add([A, B, C]);
                }

                // first based on screen
                List<Vector2> poly = new()
                 {
                    new Vector2(0, 0),
                    new Vector2(Raylib.GetScreenWidth(), 0),
                    new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()),
                    new Vector2(0, Raylib.GetScreenHeight())
                };

                // clip against half planes
                foreach (var f in functions)
                {
                    poly = ClipPoly(poly, f[0], f[1], f[2]);

                    if (poly.Count == 0)
                        break;
                }

                corners = poly;
            }
        }


        void CompareToLine(int pointIndex, Vector2 point, bool isOrigin = false)
        {
            for (int i = 0; i < functions.Count; i++)
            {
                float A = functions[i][0];
                float B = functions[i][1];
                float C = functions[i][2];

                float d = A * point.X + B * point.Y + C;

                if (isOrigin)
                    functionRelation.Add(Math.Sign(d));
                else
                {
                    if (functionRelation[i] * d < -0.0001f)
                    {
                        deadCorners.Add(corners[pointIndex]);
                        corners.RemoveAt(pointIndex);
                        break;
                    }
                }
            }
        }
        void SortCornersClockwise()
        {
            if (corners.Count < 3) return;

            // Compute center
            Vector2 center = Vector2.Zero;
            foreach (var c in corners)
                center += c;

            center /= corners.Count;

            // Create a temporary list
            List<(float, Vector2 point)> temp = new();

            foreach (var corner in corners)
            {
                float angle = MathF.Atan2(corner.Y - center.Y, corner.X - center.X);
                temp.Add((angle, corner));
            }
            temp.Sort();
            corners.Clear();
            foreach (var thing in temp)
            {
                corners.Add(thing.point);
            }
        }

        List<Vector2> ClipPoly(List<Vector2> input, float A, float B, float C)
        {
            List<Vector2> output = new();

            for (int i = 0; i < input.Count; i++)
            {
                Vector2 current = input[i];
                Vector2 prev = input[(i - 1 + input.Count) % input.Count];

                float dCurrent = A * current.X + B * current.Y + C;
                float dPrev = A * prev.X + B * prev.Y + C;

                bool currentInside = dCurrent >= 0;
                bool prevInside = dPrev >= 0;

                // Case 1: both inside
                if (currentInside && prevInside)
                {
                    output.Add(current);
                }
                // Case 2: entering
                else if (!prevInside && currentInside)
                {
                    output.Add(Intersect(prev, current, A, B, C));
                    output.Add(current);
                }
                // Case 3: leaving
                else if (prevInside && !currentInside)
                {
                    output.Add(Intersect(prev, current, A, B, C));
                }
                // Case 4: both outside → add nothing
            }

            return output;
        }
        Vector2 Intersect(Vector2 p1, Vector2 p2, float A, float B, float C)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;

            float t = -(A * p1.X + B * p1.Y + C) / (A * dx + B * dy);

            return new Vector2(
                p1.X + t * dx,
                p1.Y + t * dy
            );
        }
    }
}