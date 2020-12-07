using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day7
    {
        [Test]
        public void Solution1()
        {
            var bags = Parse();
            
            var uniqueBags = GetUniqueBags("shiny gold bags", bags, new HashSet<string>());
            
            Assert.That(uniqueBags, Is.EqualTo(326));
        }
        
        [Test]
        public void Solution2()
        {
            var bags = Parse();
            
            var bagCount = GetBagCount("shiny gold bags", bags);
            
            Assert.That(bagCount, Is.EqualTo(5635));
        }


        private int GetBagCount(string bagName, Dictionary<string, Dictionary<string, int>> bagDictionary, int totalCount = 0)
        {
            var bags = bagDictionary[bagName];
            
            var bagsCount = 0;
            foreach (var bag in bags)
            {
                var bagCount = GetBagCount(bag.Key, bagDictionary, totalCount);
                bagsCount += bag.Value + bagCount * bag.Value;
            }

            totalCount += bagsCount;

            return totalCount;
        }

        private int GetUniqueBags(string bagName, Dictionary<string,Dictionary<string, int>> bagDictionary, HashSet<string> visited)
        {
            var bags = bagDictionary.Where(innerBags => innerBags.Value.ContainsKey(bagName)).ToArray();
            
            foreach (var bag in bags)
            {
                GetUniqueBags(bag.Key, bagDictionary, visited);
            }
            
            if (!visited.Contains(bagName))
            {
                visited.Add(bagName);
            }

            return visited.Count - 1;
        }

        private Dictionary<string, Dictionary<string, int>> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day7input";
            var file = new StreamReader(filePath);

            var bagTypes = new Dictionary<string, Dictionary<string, int>>();
            string line;
            
            while ((line = file.ReadLine()) != null)
            {
                var keyValue = line.Replace(".", string.Empty).Split(" contain ");
                var values = keyValue[1].Split(", ");
                
                bagTypes.Add(keyValue[0], new Dictionary<string, int>());
                if (values[0] == "no other bags")
                {
                    continue;
                }

                foreach (var value in values)
                {
                    var countAndBag = value.Split(" ", 2);
                    var count = int.Parse(countAndBag[0]);
                    var bag = countAndBag[1];
                    
                    if (count == 1)
                    {
                        bag = $"{bag}s";
                    }
                    
                    bagTypes[keyValue[0]].Add(bag, count);
                }
            }

            file.Close();

            return bagTypes;
        }
    }
}