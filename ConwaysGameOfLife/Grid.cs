using System.Drawing;

namespace ConwaysGameOfLife
{
    public class Grid
    {
        private readonly int maxX;
        private readonly int maxY;
        public Cell[,] currentGrid;
        public Cell[,] outputGrid;
        public bool drawnVertLines = false;

        private readonly SolidBrush aliveBrush = new SolidBrush(Color.Black);
        private readonly SolidBrush deadBrush = new SolidBrush(Color.White);

        private const int MAX_LIFESPAN = int.MaxValue;
        public const int CELL_LENGTH = 5;
        public bool pause = false;

        public Grid(int _maxX, int _maxY)
        {
            maxX = _maxX / CELL_LENGTH;
            maxY = _maxY / CELL_LENGTH;
            currentGrid = new Cell[maxX, maxY];
            outputGrid = new Cell[maxX, maxY];
        }

        public void RedrawCheckGrid(Graphics gfx)
        {
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    if (!pause)
                        CheckMooreNeighbours(currentGrid[x, y]);
                    else
                        DrawCell(currentGrid[x, y], gfx);
                }
            }

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    currentGrid[x, y].alive = outputGrid[x, y].alive;
                    DrawCell(currentGrid[x, y], gfx);
                }
            }
        }

        public void InitializeGrid(Graphics gfx)
        {
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    currentGrid[x, y] = new Cell(x, y);
                    outputGrid[x, y] = new Cell(x, y);
                    if (x == maxX / 2 || y == maxY / 2)
                    {
                        currentGrid[x, y].alive = true;
                    }
                    DrawCell(currentGrid[x, y], gfx);
                }
            }
        }

        public void DrawCell(Cell cell, Graphics gfx)
        {
            Rectangle rect = new Rectangle(cell.x * CELL_LENGTH, cell.y * CELL_LENGTH, CELL_LENGTH, CELL_LENGTH);
            if (cell.alive)
            {
                gfx.FillRectangle(aliveBrush, rect);
            }
            else
                gfx.FillRectangle(deadBrush, rect);
        }

        private void CheckMooreNeighbours(Cell cell)
        {
            int liveNeighbours = 0;
            outputGrid[cell.x, cell.y].alive = cell.alive;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int neighbourNodeX = cell.x + x;
                    int neighbourNodeY = cell.y + y;
                    if (IsInGrid(neighbourNodeX, neighbourNodeY, out Cell neighbourNode))
                    {
                        if (neighbourNode.alive)
                            liveNeighbours++;
                    }
                }
            }
            CheckRules(cell, outputGrid[cell.x, cell.y], liveNeighbours);
        }

        private void CheckRules(Cell cellToCheck, Cell cellToChange, int liveNeighbours)
        {
            if (cellToCheck.alive)
            {
                if (liveNeighbours < 2 || liveNeighbours > 3 || cellToCheck.lifeSpan > MAX_LIFESPAN)
                {
                    cellToChange.alive = false;
                    cellToChange.lifeSpan = 0;
                }
                else if (liveNeighbours == 2 || liveNeighbours == 3)
                {
                    cellToChange.lifeSpan++;
                }
            }
            else if (liveNeighbours == 3)
            {
                cellToChange.alive = true;
                cellToChange.lifeSpan = 0;
            }
        }

        private bool IsInGrid(int xPos, int yPos, out Cell cell)
        {
            if (xPos >= 0 && xPos < maxX && yPos >= 0 && yPos < maxY)
            {
                cell = currentGrid[xPos, yPos];
                return true;
            }
            else
            {
                cell = null;
                return false;
            }
        }
    }
}
