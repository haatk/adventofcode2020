using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day9
    {
        [Test]
        public void Solution1()
        {
            var numbers = Parse();
            var preambleCount = 25;

            long numberToCheck = -1;
            for (var i = preambleCount; i < numbers.Count; i++)
            {
                numberToCheck = numbers[i];
                var preamble = numbers.GetRange(i - preambleCount, preambleCount);
                var valid = IsValid(numberToCheck, preamble);
                if (!valid)
                {
                    break;
                }
            }
            
            Assert.That(numberToCheck, Is.EqualTo(20874512));
        }
        
        [Test]
        public void Solution2()
        {
            var numbers = Parse();
            long numberToFind = 20874512;

            var set = FindSetBruteForce(numbers, numberToFind);
            var max = set.Max();
            var min = set.Min();

            var answer = max + min;
            Assert.That(answer, Is.EqualTo(3012420));
        }

        private List<long> FindSetBruteForce(List<long> numbers, long numberToFind)
        {
            for (var setSize = 2; setSize <= numbers.Count; setSize++)
            {
                for (var i = 0; i < numbers.Count; i++)
                {
                    if (i + setSize > numbers.Count) continue;
                    
                    var set = numbers.GetRange(i, setSize);
                    var sum = set.Sum(n => n);
                    if (sum == numberToFind)
                    {
                        return set;
                    }
                }
            }

            throw new InvalidDataException();
        }

        private bool IsValid(long numberToCheck, List<long> preamble)
        {
            return preamble.Any(first => preamble.Any(second => first != second && numberToCheck == first + second));
        }

        private List<long> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day9input";
            var file = new StreamReader(filePath);

            var numbers = new List<long>();
            string line;

            while ((line = file.ReadLine()) != null)
            {
                var value = long.Parse(line);
                
                numbers.Add(value);
            }
            
            file.Close();
            
            return numbers;
        }
    }
}