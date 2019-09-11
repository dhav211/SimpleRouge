using Godot;
using System;
using System.Collections.Generic;

public class FieldOfView : Node2D
{
    int viewDistance;
    [Export] PackedScene viewSquare;

    Grid grid;
    Player entity;

    public void InitializeFOV(Grid _grid, Player _entity, int _viewDistance)
    {
        grid = _grid;
        entity = _entity;
        viewDistance = _viewDistance;
    }

    /*
        Draw a ring around player which will be the view distance for player.
        save each vector of the ring in a list

        have a for loop that loops thru everypoint
        draw a line from entity to the point
        add every tile from the line that isn't a wall
        if line does run into a wall, then break the loop and go to next point
     */


    public void DrawFOV()  // TEST METHOD
    {
        List<Vector2> points = new List<Vector2>();
        GD.Print(entity.GridPosition);
        int x = (int)entity.GridPosition.x - 1;
        int y = (int)entity.GridPosition.y - 1;
        points = GetRingPoints(x,y,viewDistance);

        foreach (Vector2 point in points)
        {
            GD.Print(point);
            Sprite square = viewSquare.Instance() as Sprite;
            square.Position = new Vector2(point.x * 16, point.y * 16);
            AddChild(square);
        }

        /*
        for (int i = 0; i < points.Count; ++i)
        {
            List<Vector2> line = GetTilesInLine(x, (int)points[i].x, y, (int)points[i].y);

            foreach (Vector2 point in line)
            {
                Sprite square = viewSquare.Instance() as Sprite;
                square.Position = new Vector2(point.x * 16, point.y * 16);
                AddChild(square);
            }
        }
        */
    }

    private List<Vector2> GetTilesInLine(int x1, int x2, int y1, int y2)
    {
        List<Vector2> tilesInLine = new List<Vector2>();

        int dx = Math.Abs(x2 - x1), sx = x1 < x2 ? 1 : -1;
        int dy = -Math.Abs(y2 - y1), sy = y1 < y2 ? 1 : -1;
        int err = dx + dy, e2;


        for (;;)
        {
            tilesInLine.Add(new Vector2(x1,y1));

            if (x1==x2 && y1==y2) break;

            e2 = err * 2;

            if (e2 >= dy) { err += dy; x1 += sx; }
            if (e2 <= dx) { err += dx; y1 += sy; }
        }

        return tilesInLine;
    }

    private List<Vector2> GetRingPoints(int xCenter, int yCenter, int radius)  // taken from Midpoint Circle Algorithm and slightly altered to fit my needs
    { 
        List<Vector2> points = new List<Vector2>();
        int x = radius, y = 0; 
      
        // Get the initial points
        points.Add(new Vector2(x + xCenter, y + yCenter));
        points.Add(new Vector2(-x + xCenter, y + yCenter));
        points.Add(new Vector2(y + xCenter, -x + yCenter));
        points.Add(new Vector2(y + xCenter, x + yCenter));
      
        int P = 1 - radius; // initialize the perimeter
        while (x > y) 
        { 
              
            y++; 
          
            // Mid-point is inside or on the perimeter 
            if (P <= 0) 
                P = P + 2 * y + 1; 
          
            // Mid-point is outside the perimeter 
            else
            { 
                x--; 
                P = P + 2 * y - 2 * x + 1; 
            } 
            
            // All perimeter points have been created
            if (x < y) 
                break; 
            
            points.Add(new Vector2(x + xCenter, y + yCenter));
            points.Add(new Vector2(-x + xCenter, y + yCenter));
            points.Add(new Vector2(x + xCenter, -y + yCenter));
            points.Add(new Vector2(-x + xCenter, -y + yCenter));
          
            // If the generated point is on the  
            // line x = y then the perimeter points have already been created
            if (x != y)  
            {
                points.Add(new Vector2(y + xCenter, x + yCenter));
                points.Add(new Vector2(-y + xCenter, x + yCenter));
                points.Add(new Vector2(y + xCenter, -x + yCenter));
                points.Add(new Vector2(-y + xCenter, -x + yCenter));
            } 
        }

        return points;
    } 
}
