using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day11
    {
        [Test]
        public void Solution1()
        {
            var grid = Parse();

            Dictionary<int, Dictionary<int, Position>> lastGrid = null;
            Dictionary<int, Dictionary<int, Position>> currentGrid = grid;
            var roundCount = 0;
            while (!GridsAreEqual(currentGrid, lastGrid))
            {
                roundCount++;
                lastGrid = currentGrid;
                currentGrid = new Dictionary<int, Dictionary<int, Position>>();
                for (int rowIndex = 0; rowIndex < lastGrid.Count; rowIndex++)
                {
                    currentGrid.Add(rowIndex, new Dictionary<int, Position>());
                    for (int columnIndex = 0; columnIndex < lastGrid[rowIndex].Count; columnIndex++)
                    {
                        var occipiedCount = GetAdjacentOccupiedCount(lastGrid, rowIndex, columnIndex);
                        if (lastGrid[rowIndex][columnIndex] == Position.EmptyChair && occipiedCount == 0)
                        {
                            currentGrid[rowIndex].Add(columnIndex, Position.OccupiedChair);
                            continue;
                        }
                        
                        if (lastGrid[rowIndex][columnIndex] == Position.OccupiedChair && occipiedCount >= 4)
                        {
                            currentGrid[rowIndex].Add(columnIndex, Position.EmptyChair);
                            continue;
                        }
                        
                        currentGrid[rowIndex].Add(columnIndex, lastGrid[rowIndex][columnIndex]);
                    }
                }
            }

            var occupiedCount = GetOccupiedCount(currentGrid);

            Assert.AreEqual(2275, occupiedCount);
        }
        
        [Test]
        public void Solution2()
        {
            var grid = Parse();

            Dictionary<int, Dictionary<int, Position>> lastGrid = null;
            Dictionary<int, Dictionary<int, Position>> currentGrid = grid;
            var roundCount = 0;
            while (!GridsAreEqual(currentGrid, lastGrid))
            {
                roundCount++;
                lastGrid = currentGrid;
                currentGrid = new Dictionary<int, Dictionary<int, Position>>();
                for (int rowIndex = 0; rowIndex < lastGrid.Count; rowIndex++)
                {
                    currentGrid.Add(rowIndex, new Dictionary<int, Position>());
                    for (int columnIndex = 0; columnIndex < lastGrid[rowIndex].Count; columnIndex++)
                    {
                        var occipiedCount = GetFirstVisibleOccupiedCount(lastGrid, rowIndex, columnIndex);
                        if (lastGrid[rowIndex][columnIndex] == Position.EmptyChair && occipiedCount == 0)
                        {
                            currentGrid[rowIndex].Add(columnIndex, Position.OccupiedChair);
                            continue;
                        }
                        
                        if (lastGrid[rowIndex][columnIndex] == Position.OccupiedChair && occipiedCount >= 5)
                        {
                            currentGrid[rowIndex].Add(columnIndex, Position.EmptyChair);
                            continue;
                        }
                        
                        currentGrid[rowIndex].Add(columnIndex, lastGrid[rowIndex][columnIndex]);
                    }
                }
            }

            var occupiedCount = GetOccupiedCount(currentGrid);

            Assert.AreEqual(2121, occupiedCount);
        }
        
        private int GetFirstVisibleOccupiedCount(Dictionary<int, Dictionary<int, Position>> grid, int rowIndex, int columnIndex)
        {
            var firstSeatsOccupied = new[]
            {
                IsFirstSeatOccupied(grid, rowIndex, columnIndex, 1, 1),
                IsFirstSeatOccupied(grid, rowIndex, columnIndex, 1, 0),
                IsFirstSeatOccupied(grid, rowIndex, columnIndex, 0, 1),
                IsFirstSeatOccupied(grid, rowIndex, columnIndex, -1, 1),
                IsFirstSeatOccupied(grid, rowIndex, columnIndex, 1, -1),
                IsFirstSeatOccupied(grid, rowIndex, columnIndex, -1, -1),
                IsFirstSeatOccupied(grid, rowIndex, columnIndex, 0, -1),
                IsFirstSeatOccupied(grid, rowIndex, columnIndex, -1, 0),
            };

            return firstSeatsOccupied.Count(occupied => occupied);
        }

        private bool IsFirstSeatOccupied(Dictionary<int, Dictionary<int, Position>> grid, int rowIndex, int columnIndex, int deltaX, int deltaY)
        {
            var topRightRowIndex = rowIndex + deltaY;
            var topRightColumnIndex = columnIndex + deltaX;
            while (topRightColumnIndex >= 0 && topRightColumnIndex < grid[0].Count &&
                   topRightRowIndex >= 0 && topRightRowIndex < grid.Count)
            {
                var cell = grid[topRightRowIndex][topRightColumnIndex];
                if (cell == Position.EmptyChair)
                {
                    return false;
                }

                ;
                if (cell == Position.OccupiedChair)
                {
                    return true;
                }
                
                topRightRowIndex += deltaY;
                topRightColumnIndex += deltaX;
            }

            return false;
        }

        private int GetAdjacentOccupiedCount(Dictionary<int, Dictionary<int, Position>> lastGrid, int rowIndex, int columnIndex)
        {
            var occupiedCount = 0;
            for (var deltaX = -1; deltaX <= 1; deltaX++)
            {
                for (var deltaY = -1; deltaY <= 1; deltaY++)
                {
                    if (deltaX == 0 && deltaY == 0 ||
                        columnIndex + deltaX < 0 || columnIndex + deltaX >= lastGrid[0].Count ||
                        rowIndex + deltaY < 0 || rowIndex + deltaY >= lastGrid.Count)
                    {
                        continue;
                    }
                    if (lastGrid[rowIndex + deltaY][columnIndex + deltaX] == Position.OccupiedChair)
                    {
                        occupiedCount++;
                    }
                }
            }

            return occupiedCount;
        }
        
        private int GetOccupiedCount(Dictionary<int, Dictionary<int, Position>> grid)
        {
            var occupiedCount = 0;
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j] == Position.OccupiedChair)
                    {
                        occupiedCount++;
                    }
                }
            }

            return occupiedCount;
        }

        private bool GridsAreEqual(Dictionary<int, Dictionary<int, Position>> firstGrid, Dictionary<int, Dictionary<int, Position>> secondGrid)
        {
            if (firstGrid == null || secondGrid == null) return false;
            
            for (int i = 0; i < firstGrid.Count; i++)
            {
                for (int j = 0; j < firstGrid[i].Count; j++)
                {
                    if (firstGrid[i][j] != secondGrid[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        private Dictionary<int, Dictionary<int, Position>> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day11input";
            var file = new StreamReader(filePath);

            var numbers = new Dictionary<int, Dictionary<int, Position>>();
            string row;

            var rowIndex = 0;
            while ((row = file.ReadLine()) != null)
            {
                numbers.Add(rowIndex, new Dictionary<int, Position>());
                for (var columnIndex = 0; columnIndex < row.Length; columnIndex++)
                {
                    switch (row[columnIndex])
                    {
                        case 'L':
                            numbers[rowIndex].Add(columnIndex, Position.EmptyChair);
                            break;
                        case '.':
                            numbers[rowIndex].Add(columnIndex, Position.Floor);
                            break;
                        default:
                            throw new InvalidDataException();
                    }
                }

                rowIndex++;
            }
            
            file.Close();
            
            return numbers;
        }

        private enum Position
        {
            Floor,
            EmptyChair,
            OccupiedChair
        }
    }
}