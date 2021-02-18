using System;
using System.Drawing;

namespace ConwaysGameOfLife
{
    public class Grid
    {
        public readonly int maxX;
        public readonly int maxY;
        public Cell[,] currentGrid;
        public Cell[,] outputGrid;
        public bool drawnVertLines = false;

        private readonly SolidBrush aliveBrush = new SolidBrush(Color.Black);
        private readonly SolidBrush deadBrush = new SolidBrush(Color.White);

        private const int MAX_LIFESPAN = int.MaxValue;
        public const int CELL_LENGTH = 5;
        public bool pause = true;

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
                }
            }

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    if(!pause)
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
                    if (x == maxX / 2 || y == maxY / 2) // creates the cross shape
                    {
                        currentGrid[x, y].alive = true;
                    }
                    DrawCell(currentGrid[x, y], gfx);
                }
            }
            Form1.generations++;
        }

        public void DrawCell(Cell cell, Graphics gfx)
        {
            Rectangle rect = new Rectangle(cell.x * CELL_LENGTH, cell.y * CELL_LENGTH, CELL_LENGTH, CELL_LENGTH);
            if (cell.alive)
            {
                //Black cells are alive
                gfx.FillRectangle(aliveBrush, rect);
            }
            else
            {
                //White cells are dead
                gfx.FillRectangle(deadBrush, rect);
            }
        }

        private void CheckMooreNeighbours(Cell cell)
        {
            ///<summary>
            /// Here we check for the moore neighbours which are the 8 cells surrounding
            /// the given cell in square shape.
            /// 
            /// The cell parameter is always from the current grid.
            /// 
            /// We check all neighbours if they are in the grid and they are alive
            /// we add 1 to the number of live neighbours. This method is used to Check the rules
            /// and change the state of the given cells corrosponding cell in the output grid
            /// accordingly.
            ///</summary>
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
            ///<summary>
            /// These are the rules that each cell must follow in accordance
            /// to conways game of life. They can be modified to create different effects.
            /// The cell to check will always come from the current grid and the cell to change
            /// is always from the output grid.
            /// 
            /// The reason we need to only change the output grids node is so that all the nodes
            /// being changed are being affected by neighbours from the same generation.
            /// 
            /// The int liveNeighbours is used to check for the rules.
            ///</summary>
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
            ///<summary>
            /// The job of this method is to calculate whether a given x and y
            /// are contained in the current grid. If it is it will return true and 
            /// send out the cell at x and y in the current grid.
            ///</summary>
            
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
