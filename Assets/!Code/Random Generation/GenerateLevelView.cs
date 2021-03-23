using UnityEngine;
using UnityEngine.Tilemaps;


namespace Random_Generation
{
    public class GenerateLevelView : MonoBehaviour
    {
        [SerializeField] private Tilemap _groundTilemap;
        [SerializeField] private Tile _groundTile;
        [SerializeField] private int _mapWidth;
        [SerializeField] private int _mapHeight;
        [SerializeField] private int _smoothFactor;
        [SerializeField] [Range(0, 100)] private int _randomFillPercent;

        public Tilemap GroundTilemap => _groundTilemap;
        public Tile GroundTile => _groundTile;
        public int MapWidth => _mapWidth;
        public int MapHeight => _mapHeight;
        public int SmoothFactor => _smoothFactor;
        public int RandomFillPercent => _randomFillPercent;
    }
}