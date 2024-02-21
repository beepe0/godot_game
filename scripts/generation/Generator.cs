using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Generator
{
    public Grid<bool> Grid = null;
    public List<Walker> Walkers = new List<Walker>() { new Walker() };
    private List<Walker> WalkersToAdd = new List<Walker>();

    public int CurrentRoomsCount { get; private set; } = 0;

    public int TargetRoomsCount = 10;
    
    

    public void Update()
    {
        foreach (Walker walker in Walkers)
        {
            if (CurrentRoomsCount >= TargetRoomsCount)
            {
                break;
            }

            if (Grid[walker.Position] == false)
            {
                Grid[walker.Position] = true;
                CurrentRoomsCount++;
            }

            List<Vector2I> validNeighbours = new List<Vector2I>();

            
            foreach (Vector2I neighbor in Grid.GetNeighborsWith(walker.Position, Neighborhood.Manhattan, false))
            {
                if(Grid.GetNeighborsWith(neighbor, Neighborhood.Moore, true).Count < 4)
                {
                    validNeighbours.Add(neighbor);
                }
            }

            if (validNeighbours.Count > 0)
            {
                walker.MoveHistory.Push(walker.Position);

                Vector2I targetPosition = validNeighbours[Game.Instance.Rng.RandiRange(0, validNeighbours.Count - 1)];

                if (Game.Instance.Rng.Randf() <= 0.05f)
                {
                    Walker newWalker = new Walker();

                    newWalker.Position = targetPosition;
                    newWalker.MoveHistory = new Stack<Vector2I>(walker.MoveHistory);

                    WalkersToAdd.Add(newWalker);
                }

                else
                {
                    walker.Position = targetPosition;
                }

                
                
            }

            else if(walker.MoveHistory.Count > 0)
            {
                walker.Position = walker.MoveHistory.Pop();
                
            }
        }
        Walkers.AddRange(WalkersToAdd);
        WalkersToAdd.Clear();
    }

    public void Generate() {
        foreach (Walker walker in Walkers)
        {
            walker.Position = Grid.Size / 2;
        }

        while (CurrentRoomsCount < TargetRoomsCount) Update();
    }
    
}
