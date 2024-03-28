using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BP.GameConsole;

public partial class DungeonTileCategoryFinish : DungeonTileCategory
{
    public override async Task<List<DungeonTile>> Execute(DungeonTile tile, bool isRemove)
    {
        try
        {
            DungeonTileFinish tileFinish = tile as DungeonTileFinish;
            if (tileFinish.SecondConnectors.Count > 0)
            {
                tileFinish.Connectors.Clear();
                tileFinish.Connectors.AddRange(tileFinish.SecondConnectors);
                tileFinish.SecondConnectors.Clear();
            }
            var neighbours = await base.Execute(tileFinish, isRemove);
            
            if (!tileFinish.CanBeAdded)
            {
                DungeonBuilder.ValidTilesFrom.Add(tileFinish);
                tileFinish.CanBeAdded = true;
            }
            
            return neighbours;
        }
        catch (Exception e)
        {
            GameConsole.Instance.DebugError($"Tile \"{tile.Category}\", {e}");
            return null;
        }
    }
}