using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day13
    {
        [Test]
        public void Solution1()
        {
            var busSchedule = Parse();
            var busIds = busSchedule.BusIds.Where(busId => busId != "x").Select(int.Parse).ToList();

            var lowestBus = int.MaxValue;
            var lowestDifference = int.MaxValue; 
            foreach (var busId in busIds)
            {
                var firstBusForId = (int) Math.Ceiling((double) busSchedule.EarliestTimestamp / busId);
                var difference = firstBusForId * busId - busSchedule.EarliestTimestamp;
                if (difference < lowestDifference)
                {
                    lowestBus = busId;
                    lowestDifference = difference;
                }
            }

            var answer = lowestBus * lowestDifference;
            Assert.AreEqual(3882, answer);
        }

        [Test]
        public void Solution2()
        {
            var list = Parse().BusIds;

            var busList = list.Select((busId, busOffset) => (busId, busOffset));
            List<(long busId, int busOffset)> sets = busList
                .Where(set => set.busId != "x")
                .Select(set => (long.Parse(set.busId), set.busOffset))
                .OrderByDescending(x => x.Item1).ToList();
            
            var stepCount = sets[0].busId;
            var time = sets[0].busId - sets[0].busOffset;
            
            for (var i = 1; i <= sets.Count; i++)
            {
                var setPart = sets.Take(i).ToList();
                
                while (setPart.Any(set => (time + set.busOffset) % set.busId != 0))
                {
                    time += stepCount;
                }

                stepCount = setPart.Select(t => t.busId).Aggregate(Lcm);
            }

            Assert.AreEqual(867295486378319, time);
        }
        
        private long Gcd(long m, long n)
        {
            while (n != 0)
            {
                var temp = n;
                n = m % n;
                m = temp;
            }
            return m;
        }
        
        private long Lcm(long m, long n)
        {
            return m * n / Gcd(m, n);
        }
        
        private BusSchedule Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day13input";
            var file = new StreamReader(filePath);

            var busSchedule = new BusSchedule();
            string line;

            var lineIndex = 0;
            
            while ((line = file.ReadLine()) != null)
            {
                switch (lineIndex)
                {
                    case 0:
                        busSchedule.EarliestTimestamp = int.Parse(line);
                        break;
                    case 1:
                    {
                        var busScheduleBusIds = line.Split(",").ToList();
                        busSchedule.BusIds = busScheduleBusIds;
                        break;
                    }
                }

                lineIndex++;
            }
            
            file.Close();
            
            return busSchedule;
        }

        private class BusSchedule
        {
            public int EarliestTimestamp { get; set; }
            public IList<string> BusIds { get; set; }
        }
    }
}