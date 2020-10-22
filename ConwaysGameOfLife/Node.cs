namespace ConwaysGameOfLife
{
    public class Cell
    {
        public Cell(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public bool alive = false;
        public int x;
        public int y;

        public int lifeSpan = 0;
    }
}
