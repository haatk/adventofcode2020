using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day3
    {
        [Test]
        public void Solution1()
        {
            var result = GetTreeEncounterCount(3, 1);
            Assert.That(result, Is.EqualTo(216));
        }

        [Test]
        public void Solution2()
        {
            var list = new long[]
            {
                GetTreeEncounterCount(1, 1),
                GetTreeEncounterCount(3, 1),
                GetTreeEncounterCount(5, 1),
                GetTreeEncounterCount(7, 1),
                GetTreeEncounterCount(1, 2),
            };

            var result = list.Aggregate((x, y) => y * x);

            Assert.That(result, Is.EqualTo(6708199680));
        }

        private int GetTreeEncounterCount(int stepX, int stepY)
        {
            var grid = Parse();

            var gridWidth = grid[0].Count;
            var encounterCount = 0;
            var x = 0;
            var y = 0;
            while (y < grid.Count)
            {
                var moduloX = x % gridWidth;
                var hasTree = grid[y][moduloX];
                if (hasTree) encounterCount++;
                
                y += stepY;
                x += stepX;
            }

            return encounterCount;
        }

        private Dictionary<int, Dictionary<int, bool>> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day3input";
            var file = new StreamReader(filePath);
            
            var gridDictionary = new Dictionary<int, Dictionary<int, bool>>();
            string line;
            var lineCount = 0;
            
            while ((line = file.ReadLine()) != null)
            {
                var row = line
                    .Select((character, index) => new {Item = character, Index = index})
                    .ToDictionary(c => c.Index, c => c.Item == '#');

                gridDictionary.Add(lineCount, row);

                lineCount++;
            }

            file.Close();

            return gridDictionary;
        }
    }
}