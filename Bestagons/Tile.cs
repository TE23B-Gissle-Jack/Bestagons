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
        List<float[]> functions = new List<float[]>(); // [A, B, C]
        List<float> functionRelation = new List<float>();
        public static bool hideLines = false;
        bool hovered = false;
        Color lineColor = new(122 + Random.Shared.Next(122), 122 + Random.Shared.Next(122), 122 + Random.Shared.Next(122));

        public Tile(Vector2 position)
        {
            orgin = position;
            tilePositions.Add(position);
        }

        public void Draw()
        {
            Raylib.DrawCircleV(orgin, 5, Color.Red);
            if (!hideLines||hovered)
            {
                if(corners.Count > 1)
                {
                    Raylib.DrawLineV(corners[0], corners[corners.Count - 1], lineColor);
                }
                
                for (int i = 1; i < corners.Count; i++)
                {
                    Raylib.DrawLineV(corners[i], corners[i - 1], lineColor);
                }
                foreach (var item in corners)
                {
                    Raylib.DrawCircleV(item, 5, Color.Pink);
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

                float A, B, C;

                if (Math.Abs(dx) < 0.0001f)
                {
                    A = 0; B = 1; C = -middle.Y;
                }
                else if (Math.Abs(dy) < 0.0001f)
                {
                    A = 1; B = 0; C = -middle.X;
                }
                else
                {
                    float k = dy / dx;
                    float k2 = -1 / k;
                    float m = middle.Y - k2 * middle.X;

                    // y = k2*x + m → k2*x - y + m = 0
                    A = k2; B = -1; C = m;
                }

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
                    if (functionRelation[i] * d < 0)
                    {
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
    }
}