using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day2
    {
        private Regex _regex = 
        new Regex(@"(?'lowerLimit'\d*)-(?'upperLimit'\d*) (?'character'[a-z]{1}): (?'password'.*)");

        [Test]
        public void Solution1()
        {
            var passwordSets = Parse();
            var validCount = 0;
            foreach (var passwordSet in passwordSets)
            {
                var count = passwordSet.Password.Count(character => character == passwordSet.Character);
                if (count >= passwordSet.Lower && count <= passwordSet.Upper)
                {
                    validCount++;
                }
            }
            
            Assert.That(validCount, Is.EqualTo(625));
        }
        
        [Test]
        public void Solution2()
        {
            var passwordSets = Parse();
            var validCount = 0;
            foreach (var passwordSet in passwordSets)
            {
                var matchLower = passwordSet.Password[passwordSet.Lower - 1] == passwordSet.Character;
                var matchUpper = passwordSet.Password[passwordSet.Upper - 1] == passwordSet.Character;

                if (matchLower ^ matchUpper) validCount++;
            }
            
            Assert.That(validCount, Is.EqualTo(391));
        }

        private List<PasswordSet> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day2input";
            var file = new StreamReader(filePath); 
            
            var passwordSets = new List<PasswordSet>();
            string line;
            
            while((line = file.ReadLine()) != null)
            {
                var match = _regex.Match(line);
                var passwordSet = new PasswordSet
                {
                    Character = match.Groups["character"].Value.First(),
                    Lower = int.Parse(match.Groups["lowerLimit"].Value),
                    Upper = int.Parse(match.Groups["upperLimit"].Value),
                    Password = match.Groups["password"].Value,
                };

                passwordSets.Add(passwordSet);
            }  
  
            file.Close();

            return passwordSets;
        }
    }

    internal class PasswordSet
    {
        public int Lower { get; set; }
        public int Upper { get; set; }
        public char Character { get; set; }
        public string Password { get; set; }
    }
}