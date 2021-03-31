


using UnityEngine;


namespace Random_Generation
{
    public class SquareGrid
    {
        public Square[,] Squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            var nodeCountX = map.GetLength(0);
            var nodeCountY = map.GetLength(1);
            var mapWidth = nodeCountX * squareSize;
            var mapHeight = nodeCountY * squareSize;

            var controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (var x = 0; x < nodeCountX; x++)
            {
                for (var y = 0; y < nodeCountY; y++)
                {
                    var position = new Vector3(-mapWidth/2 + x * squareSize + squareSize/2, -mapHeight/2 + y *squareSize +squareSize/2);
                    controlNodes[x,y] =new ControlNode(position, map[x,y] == 1);
                }
            }
          
            Squares = new Square[nodeCountX - 1, nodeCountY - 1];
          
            for (var x = 0; x < nodeCountX - 1; x++)
            {
                for (var y = 0; y < nodeCountY - 1; y++)
                {
                    Squares[x,y] = new Square(controlNodes[x, y+1], controlNodes[x+1, y+1],
                        controlNodes[x+1, y], controlNodes[x,y]);
                }
            }
        }
    }
}