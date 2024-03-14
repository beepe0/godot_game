using Godot;
using System.Collections.Generic;

public class MapWalker
{
    public Stack<Vector2I> MoveHistory = new Stack<Vector2I>();

    public MapGenerator MapGenerator;
    public Vector2I Position;
    public ushort CurrentRoom;
    public ushort CurrentLevel;
    public bool IsDone;
    
    public void Dig()
    {
        if (CurrentRoom > 0)
        {
            List<Vector2I> validNeighbours = new List<Vector2I>();
            List<Vector2I> tempNeighbours = MapGenerator.MapGrid.GetNeighborsWith(CurrentLevel, Position, Neighborhood.Manhattan, MapTile.Closed);
        
            foreach (Vector2I neighbor in tempNeighbours)
            {
                if(MapGenerator.MapGrid.GetNeighborsWith(CurrentLevel, neighbor, Neighborhood.Moore, MapTile.Opened).Count < 4)
                {
                    validNeighbours.Add(neighbor);
                }
            }

            if (validNeighbours.Count > 0)
            {
                MoveHistory.Push(Position);
                Vector2I targetPosition = validNeighbours[MapGenerator.Random.RandiRange(0, validNeighbours.Count - 1)];
                
                if (CurrentLevel > 1 && MapGenerator.Random.Randf() <= 0.02f)
                {
                    MapWalker newMapWalker = new MapWalker
                    {
                        MapGenerator = MapGenerator,
                        Position = Position,
                        MoveHistory = new Stack<Vector2I>(MoveHistory)
                    };
            
                    MapGenerator.Walkers.Add(newMapWalker);
                }
                else
                {
                    Position = targetPosition;
                }
            }
            else if(MoveHistory.Count > 0)
            {
                Position = MoveHistory.Pop();
            }
            else
            {
                IsDone = true;
                return;
            }
        }
        
        var cell = MapGenerator.MapGrid[Position, CurrentLevel];
        if (cell.State == false)
        {
            CurrentRoom++;
            cell.Id = MapGenerator.CurrentRoomsCount++;
            cell.State = true;
            if (CurrentRoom >= MapGenerator.TargetRoomsCountPerWalker && CurrentLevel < MapGenerator.MapGrid.Size.Z - 1)
            {
                MapGenerator.MapGrid[Position, CurrentLevel].SubCat = 'f'; 
                MapGenerator.MapGrid[Position, (ushort)(CurrentLevel + 1)].SubCat = 'm'; 
            }
        }
        if (CurrentRoom >= MapGenerator.TargetRoomsCountPerWalker)
        {
            CurrentRoom = 0;
            CurrentLevel++;
            if (CurrentLevel >= MapGenerator.MapGrid.Size.Z)
            {
                IsDone = true;
                return;
            }
        }
    }
}