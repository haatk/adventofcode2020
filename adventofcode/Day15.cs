

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day15
    {
        [Test]
        public void Solution1()
        {
            var answer = GetNthNumber(2020);

            Assert.That(answer, Is.EqualTo(371));
        }
        
        [Test]
        public void Solution2()
        {
            var answer = GetNthNumber(30000000);

            Assert.That(answer, Is.EqualTo(352));
        }

        private int GetNthNumber(int n)
        {
            var numbers = new List<int>{9, 3, 1, 0, 8, 4};
        
            var initialCount = numbers.Count;
            var lastOccurrence = new Dictionary<int, int>();

            for (var i = 0; i < numbers.Count; i++)
            {
                var number = numbers[i];
                lastOccurrence.Add(number, i);
            }

            for (var lastIndex = initialCount - 1; lastIndex < n - 1; lastIndex++)
            {
                var lastNumber = numbers[lastIndex];

                if (!lastOccurrence.ContainsKey(lastNumber))
                {
                    numbers.Add(0);
                    lastOccurrence.Add(lastNumber, lastIndex);
                }
                else
                {
                    var index = lastOccurrence[lastNumber];
                    numbers.Add(lastIndex - index);
                    lastOccurrence[lastNumber] = lastIndex;
                }
            }

            return numbers.Last();
        }
    }
}