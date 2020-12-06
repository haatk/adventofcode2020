using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day6
    {
        [Test]
        public void Solution1()
        {
            var answerGroups = Parse();
            
            var total = answerGroups.Sum(answerGroup => string.Join(string.Empty, answerGroup).Distinct().Count());

            Assert.That(total, Is.EqualTo(6703));
        }
        
        [Test]
        public void Solution2()
        {
            var answerGroups = Parse();
            
            var total = 0;
            foreach (var answerGroup in answerGroups)
            {
                var chars = answerGroup[0].Distinct();
                for (var i = 1; i < answerGroup.Count; i++)
                {
                    chars = chars.Intersect(answerGroup[i]);
                }
                
                total += chars.Count();
            }
            
            Assert.That(total, Is.EqualTo(3430));
        }
        
        private List<List<string>> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day6input";
            var file = new StreamReader(filePath);

            var answerGroups = new List<List<string>>();
            string line;
            var answerGroup = new List<string>();
            
            while ((line = file.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    answerGroups.Add(answerGroup);
                    answerGroup = new List<string>();
                    continue;
                }

                answerGroup.Add(line);
            }
            answerGroups.Add(answerGroup);

            file.Close();

            return answerGroups;
        }
    }
}