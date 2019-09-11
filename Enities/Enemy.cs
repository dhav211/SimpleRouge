using Godot;
using System;
using System.Collections.Generic;

public class Enemy : Node2D
{
    private enum EnemyState { Wandering, Sleeping, Pursuit, Fleeing }
    private EnemyState currentEnemyState = EnemyState.Wandering;

    Vector2 gridPosition = new Vector2();
    bool isAlive = true;
    [Export] int viewDistance = 8;
    [Export] PackedScene squareScene;  // TODO delete this

    TurnManager turnManager;
    Pathfinding pathfinding = new Pathfinding();
    Grid grid;
    Player player;
    Game game;
    Stats stats;
    Attack attack = new Attack();

    public Stats Stats
    {
        get { return stats; }
    }

    public bool IsAlive
    {
        get { return isAlive; }
    }

    public override void _Ready()
    {
        turnManager = GetNode("/root/TurnManager") as TurnManager;
        grid = GetTree().GetRoot().GetNode("Game/Grid") as Grid;
        game = GetTree().GetRoot().GetNode("Game") as Game;
        player = GetTree().GetRoot().GetNode("Game/Player") as Player;
        stats = GetNode("Stats") as Stats;
        turnManager.Turns.Add(this);
        pathfinding.InitializePathfinding(grid, player);

        SpawnInRandomPosition();
        gridPosition = new Vector2(Mathf.FloorToInt(Position.x /16), Mathf.FloorToInt(Position.y /16));
    }

    public void RunAI()
    {
        if (currentEnemyState == EnemyState.Wandering)
        {
            bool isInViewDistance = IsPlayerInViewDistance();
            bool isInLineOfSight = false;

            if (isInViewDistance)  // Run line of sight algorithim if player is nearby, if not don't waste the energy
                isInLineOfSight = IsPlayerInLineOfSight();

            if (!isInViewDistance || !isInLineOfSight)
            {
                Wander();
            }
            
            else if (isInLineOfSight)
            {
                PlayerPlayerSpottedAnimation();
                currentEnemyState = EnemyState.Pursuit;
            }
        }
        else if (currentEnemyState == EnemyState.Pursuit)
        {
            if (IsPlayerNearby())
            {
                attack.AttackTarget(this, player);
            }
            else
            {
                ChasePlayer();
            }
        }
        ChangeGridPosition();
    }

    private bool IsPlayerNearby()
    {
        // Gets all the 8 surrounding tile positions, then checks each one with players grid position, if one matches, then the player is nearby

        Vector2[] surroundingTiles = GetSurroundingTilesPosition(gridPosition);

        for (int i = 0; i < surroundingTiles.Length; ++i)
        {
            if (surroundingTiles[i] == player.GridPosition)  // The player is in this square
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPlayerInViewDistance()
    // Checks if player is within viewing range, then returning a true or false
    {
        Vector2 distanceFromPlayer = gridPosition - player.GridPosition;
        if (Math.Abs(distanceFromPlayer.x) <= viewDistance && Math.Abs(distanceFromPlayer.y) <= viewDistance)
        {
            return true;
        }
        return false;
    }

    private bool IsPlayerInLineOfSight()
    {
        // Forms a line of tiles from enemy to player, then will check each tile to see if it's a wall or not. If it's a wall, then enemy can't see the player.

        List<Vector2> lineOfSight = GetLineOfSight((int)gridPosition.x, (int) player.GridPosition.x, (int)gridPosition.y, (int) player.GridPosition.y);

        foreach (Vector2 point in lineOfSight)
        {
            int x = (int)point.x;
            int y = (int)point.y;

            if (grid.TileGrid[x,y].SelectedTypeOfTile == Tile.TypeOfTile.Wall)
            {
                GD.Print("Theres a wall here, enemy cant see the player");
                return false;
            }
        }

        GD.Print("The view was clear, enemy sees the player");
        return true;
    }

    private List<Vector2> GetLineOfSight(int x1, int x2, int y1, int y2)
    // Uses Bresenahm's Line Algoritim to get every tile from enemy to player
    {
        List<Vector2> lineOfSight = new List<Vector2>();

        int dx = Math.Abs(x2 - x1), sx = x1 < x2 ? 1 : -1;
        int dy = -Math.Abs(y2 - y1), sy = y1 < y2 ? 1 : -1;
        int err = dx + dy, e2;


        for (; ; )
        {
            lineOfSight.Add(new Vector2(x1, y1));

            if (x1 == x2 && y1 == y2) break;

            e2 = err * 2;

            if (e2 >= dy) { err += dy; x1 += sx; }
            if (e2 <= dx) { err += dx; y1 += sy; }
        }

        return lineOfSight;
    }

    private void Wander()
    {
        // Randomly choose a spot to move to, if it is occupied or is not a floor, then choose another spot. There is an attempt int to limit the amount of times
        // the enemy will try, just so it won't get stuck in an infinite loop

        bool isChoosingSurroundingTile = true;
        int attempts = 0;
        int maxAttempts = 5;
        Vector2[] surroundingTiles = GetSurroundingTilesPosition(gridPosition);

        while (isChoosingSurroundingTile || attempts < maxAttempts)
        {
            int randomChoice = game.Random.Next(0, surroundingTiles.Length);
            Tile tileToChoose = grid.TileGrid[(int)surroundingTiles[randomChoice].x, (int)surroundingTiles[randomChoice].y];
            
            if (!tileToChoose.IsOccupied && tileToChoose.SelectedTypeOfTile == Tile.TypeOfTile.Floor)
            {
                Position = tileToChoose.Position;
                isChoosingSurroundingTile = false;
            }
            attempts++;
        }
    }

    private void ChasePlayer()
    {
        //  Find the path to the player, then move to the first tile of the path.

        List<Vector2> pathToFollow = pathfinding.StartPathfinding(gridPosition, player.GridPosition);
        Position = new Vector2(pathToFollow[0].x * 16, pathToFollow[0].y * 16);
    }

    private Vector2[] GetSurroundingTilesPosition(Vector2 _gridPosition)
    {   
        // Fills and returns an array with the positions of all 8 surrounding tiles
        Vector2 [] surroundingTiles = new Vector2[8];

        surroundingTiles[0] = _gridPosition - new Vector2(-1, -1);
        surroundingTiles[1] = _gridPosition - new Vector2(0, -1);
        surroundingTiles[2] = _gridPosition - new Vector2(1, -1);
        surroundingTiles[3] = _gridPosition - new Vector2(1, 0);
        surroundingTiles[4] = _gridPosition - new Vector2(1, 1);
        surroundingTiles[5] = _gridPosition - new Vector2(0, 1);
        surroundingTiles[6] = _gridPosition - new Vector2(-1, 1);
        surroundingTiles[7] = _gridPosition - new Vector2(-1, 0);

        return surroundingTiles;
    }

    private void ChangeGridPosition()
    {
        // Changes grid position of player and changes IsOccupied bool in tileGrid.
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = false;  // sets isOccupied on old grid position on tileGrid to false
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = null;

        gridPosition = new Vector2(Mathf.FloorToInt(Position.x /16), Mathf.FloorToInt(Position.y /16));  // Change gridPosition based upon current Position

        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = true; // sets isOccupied on old grid position on tileGrid to true
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = this;
    }
    
    private void PlayerPlayerSpottedAnimation()
    {
        AnimationPlayer anim = GetNode("StatusIndicatorLocation/PlayerSpottedIndicator/AnimationPlayer") as AnimationPlayer;
        anim.Play("ShowStatus");
    }

    private void SpawnInRandomPosition()
    {
        // Spawns the Enemy somewhere on the map by choosing a random location, then checking if it's a floor tile, if it isn't it tries again, if it is then it spawns
        // TODO check to see if the tile is occupied, also when spawned, set that tile as occupied

        Random random = grid.Random;
        bool hasFoundSpawnLocation = false;

        while(!hasFoundSpawnLocation)
        {
            Vector2 spawnLocation = new Vector2(random.Next(0, grid.GridWidth - 1), random.Next(0, grid.GridHeight - 1));

            if (grid.TileGrid[(int)spawnLocation.x, (int)spawnLocation.y].SelectedTypeOfTile == Tile.TypeOfTile.Floor)
            {
                Position = new Vector2(spawnLocation.x * 16, spawnLocation.y * 16);
                gridPosition = new Vector2(spawnLocation.x, spawnLocation.y);
                hasFoundSpawnLocation = true;
            }
        }
    }

    public void CheckIfAlive()
    {
        // Checks if enemy is still alive, if it has died, this will play the death animation, which after animation, the enemy will be made invisble.
        if (stats.CurrentHealth <= 0)
        {
            isAlive = false;
            AnimationPlayer anim = GetNode("AnimationPlayer") as AnimationPlayer;
            anim.Play("Death");
        }
    }

    public void RemoveEnemy()
    {
        // Doesn't actually remove enemy fully. Just makes it invisible and removes it from tile grid.
        // Function is called within the animation player, right at the end of the Death animation
        Visible = false;
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = false; // sets isOccupied on old grid position on tileGrid to true
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = null;
    }

    private void _on_Enemy_turn_completed()
    {

    }
}
