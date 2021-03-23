namespace Random_Generation
{
    public class Square
    {
        public ControlNode 
            TopLeft, 
            TopRight, 
            BottomRight, 
            BottomLeft;

        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;
        }
    }
}