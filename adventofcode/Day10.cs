using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day10
    {
        [Test]
        public void Solution1()
        {
            var adapterJolts = Parse().OrderBy(a => a).ToList();

            var singleJoints = adapterJolts[0];
            var tripleJoints = 1;

            for (var index = 0; index < adapterJolts.Count - 1; index++)
            {
                var adapterJolt = adapterJolts[index];
                var nextAdapterJolt = adapterJolts[index + 1];

                switch (nextAdapterJolt - adapterJolt)
                {
                    case 1:
                        singleJoints++;
                        break;
                    case 3:
                        tripleJoints++;
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }

            var sum = tripleJoints * singleJoints;
            Assert.AreEqual(2040, sum);
        }
        
        private List<int> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day10input";
            var file = new StreamReader(filePath);

            var numbers = new List<int>();
            string line;

            while ((line = file.ReadLine()) != null)
            {
                var value = int.Parse(line);
                
                numbers.Add(value);
            }
            
            file.Close();
            
            return numbers;
        }
    }
}