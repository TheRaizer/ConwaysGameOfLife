# The Steps Taken for my implementation of Conway's Game of Life

{% include youtubePlayer.html id="hkVPwFVoCUw" %} 



 
## The Classes
### The Cell Class
This class manages each individual cell. It contains the position of the cell, the life span and whether it is alive or not. The cell is represented as a black square with the dimensions set as a constant.

```csharp
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
```



### The Grid Class
The grid class manages every Cell in a grid whose size is dependent on the windows form screen size. It initializes the grid and is in charge of evaluating the state of the cells within the grid given a rule set.

```csharp
public class Grid
    {
        public readonly int maxX;
        public readonly int maxY;
        public Cell[,] currentGrid;
        public Cell[,] outputGrid;
        public bool drawnVertLines = false;

        private readonly SolidBrush aliveBrush = new SolidBrush(Color.Black);
        private readonly SolidBrush deadBrush = new SolidBrush(Color.White);

        private const int MAX_LIFESPAN = int.MaxValue; // This value can be modified. It is set as max value so life span does not affect the cells.
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


            ///<summary>
            /// Must make sure to replace all the current grid cells with the output grid cells
            /// before checking the neighbours. The reason for doing this is so all cells are being
            /// affected only by cells in the same generation.
            ///</summary>

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
```




# Breaking Down the Grid Class
## Initialization of the Grid
To initialize the grid I simply loop through the height of the window(maxY) and the width of the window(maxX) creating a Cell for both the output and current grid. The If statement is only used to display the cross that you see at the beginning of the video.

```csharp
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
```

## Drawing Cells
To draw cells onto the screen we use this function. It takes a given cell as well as the Graphics class from the windows form. We then generate a rectangle based on the x and y of the given cell which will be used to call FillRectangle(). But before doing that we check if the cell is alive or not. If it is alive we draw it with the aliveBrush(black) and if it is dead we draw it with the deadBrush(white).

```csharp
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
```
 


## Checking For Neighbours
In my implementation I decided to use Moore neighbours instead of Von Neumann. This means that I account for the eight surrounding squares. The code below is how I scan for the Moore neighbours around each cell in the grid. If a neighbour is found and is alive I will add to the number of live neighbours. This count of neighbours will be later used to evaluate the rule set of my choice.

```csharp
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
```

 
## The Rule Set
The Cells in my grid act according to these rules.
1. If the current cell is alive and has less than 2 live neighbours it dies of under population
2. If the current cell is alive and has more then 3 live neighbours it dies of over population
3. If the current cell has passed its life span it will die
4. If the current cell is dead and it has exactly 3 live neighbours it comes to life as if from reproduction
5. If the current cell has either 2 or 3 neighbours it will age

```csharp
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
```
 
## Checking in the Grid
This is a simple function that returns a boolean of whether the given cell is within the bounds of the grid. It is used to make sure that we don't attempt to check for neighbour cells outside of the grid.

```csharp
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
```
 
## Redrawing the Grid
This is a function that will be called everytime the windows forms is redrawn onto the screen. That is also why it is given the graphics object. Its job is to run the CheckMooreNeighbours() method on every node in the grid. After that it means that the output grid has been properly changed to fit what the next generation would look like. We then loop through each cell but this time we individually change the values of the current grid to the be equivalent to the output grid while also drawing each cell. The reason we need two seperate pairs of 2 for loops is because we don't want to change what is in the current grid while still checking for neighbours. What this will do is cause cells from the next generation to affect how the cells from the current generation respond to the rule set. We could involve multi-threading however the Graphics object from a windows forms doesn't easily support multi-threading or Async programming.

```csharp
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
```
