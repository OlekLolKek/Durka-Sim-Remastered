using UnityEngine;
using UnityEngine.Tilemaps;

namespace Random_Generation
{
   public class LevelGeneratorController
   {
       private readonly MarchingSquaresLevelGenerator _marchingSquaresLevelGenerator = new MarchingSquaresLevelGenerator();
       
       private const int WALL_COUNT = 4;

       private readonly Tilemap _groundTilemap;
       private readonly Tile _groundTile;
       private readonly int _mapWidth;
       private readonly int _mapHeight;
       private readonly int _smoothFactor;
       private readonly int _randomFillPercent;

       private readonly int[,] _map;

       public LevelGeneratorController(GenerateLevelView generateLevelView)
       {
           _groundTilemap = generateLevelView.GroundTilemap;
           _groundTile = generateLevelView.GroundTile;
           _mapWidth = generateLevelView.MapWidth;
           _mapHeight = generateLevelView.MapHeight;
           _smoothFactor = generateLevelView.SmoothFactor;
           _randomFillPercent = generateLevelView.RandomFillPercent;

           _map = new int[_mapWidth, _mapHeight];
       }

       public void Awake()
       {
           GenerateLevel();
       }

       // private void GenerateLevel()
       // {
       //     RandomFillLevel();
       //
       //     for (var i = 0; i < _smoothFactor; i++)
       //         SmoothMap();
       //
       //     DrawTilesOnMap();
       // }

       private void GenerateLevel()
       {
           RandomFillLevel();

           for (int i = 0; i < _smoothFactor; i++)
           {
               SmoothMap();
           }

           _marchingSquaresLevelGenerator.GenerateGrid(_map, 1);
           _marchingSquaresLevelGenerator.DrawTilesOnMap(_groundTilemap, _groundTile);
       }

       private void RandomFillLevel()
       {
           var seed = new System.Random().Next();
           var random = new System.Random(seed.GetHashCode());

           for (var x = 0; x < _mapWidth; x++)
           {
               for (var y = 0; y < _mapHeight; y++)
               {
                   if (x == 0 || x == _mapWidth - 1 || y == 0 || y == _mapHeight - 1)
                       _map[x, y] = 1;
                   else
                       _map[x, y] = (random.Next(0, 100) < _randomFillPercent) ? 1 : 0;
               }
           }
       }

       private void SmoothMap()
       {
           for (var x = 0; x < _mapWidth; x++)
           {
               for (var y = 0; y < _mapHeight; y++)
               {
                   var neighbourWallTiles = GetSurroundingWallCount(x, y);

                   if (neighbourWallTiles > WALL_COUNT)
                       _map[x, y] = 1;
                   else if (neighbourWallTiles < WALL_COUNT)
                       _map[x, y] = 0;
               }
           }
       }

       private int GetSurroundingWallCount(int gridX, int gridY)
       {
           var wallCount = 0;

           for (var neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
           {
               for (var neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
               {
                   if (neighbourX >= 0 && neighbourX < _mapWidth && neighbourY >= 0 && neighbourY < _mapHeight)
                   {
                       if (neighbourX != gridX || neighbourY != gridY)
                           wallCount += _map[neighbourX, neighbourY];
                   }
                   else
                   {
                       wallCount++;
                   }
               }
           }

           return wallCount;
       }

       private void DrawTilesOnMap()
       {
           if (_map == null)
               return;

           for (var x = 0; x < _mapWidth; x++)
           {
               for (var y = 0; y < _mapHeight; y++)
               {
                   var positionTile = new Vector3Int(-_mapWidth / 2 + x, -_mapHeight / 2 + y, 0);

                   if (_map[x, y] == 1)
                       _groundTilemap.SetTile(positionTile, _groundTile);
               }
           }
       }
   }
}

