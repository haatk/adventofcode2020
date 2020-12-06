using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day4
    {
        [Test]
        public void Solution1()
        {
            var passports = Parse().ToList();

            var validPassports = passports.Count(IsValid);
            
            Assert.That(validPassports, Is.EqualTo(219));
        }
        
        [Test]
        public void Solution2()
        {
            var passports = Parse().ToList();

            var validPassports = passports.Count(IsFullyValid);
            
            Assert.That(validPassports, Is.EqualTo(127));
        }
        
        private bool IsFullyValid(Dictionary<string, string> dictionary)
        {
            if (!IsValid(dictionary)) return false;

            var byr = int.Parse(dictionary["byr"]);
            if (byr < 1920 || byr > 2002)
            {
                return false;
            }
            
            var iyr = int.Parse(dictionary["iyr"]);
            if (iyr < 2010 || iyr > 2020)
            {
                return false;
            }
            
            var eyr = int.Parse(dictionary["eyr"]);
            if (eyr < 2020 || eyr > 2030)
            {
                return false;
            }

            var metric = dictionary["hgt"].Substring(dictionary["hgt"].Length - 2);
            if (!string.Equals(metric, "cm") && !string.Equals(metric, "in"))
            {
                return false;
            }
            var hgt = int.Parse(dictionary["hgt"].Replace(metric, ""));
            if (metric == "cm" && (hgt < 150 || hgt > 193))
            {
                return false;
            }
            if (metric == "in" && (hgt < 59 || hgt > 76))
            {
                return false;
            }

            var hcl = dictionary["hcl"];
            var hexRegex = new Regex("^#[a-fA-F0-9]{6}$", RegexOptions.IgnoreCase);
            if (!hexRegex.IsMatch(hcl))
            {
                return false;
            }
            
            var ecl = dictionary["ecl"];
            var validColors = new[]
            {
                "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
            };
            if (!validColors.Contains(ecl))
            {
                return false;
            }
            
            var digitRegex = new Regex(@"^[0-9]{9}$");
            var pid = dictionary["pid"];
            if (!digitRegex.IsMatch(pid))
            {
                return false;
            }

            return true;
        }

        private bool IsValid(Dictionary<string, string> dictionary)
        {
            var requiredFields = new List<string>
            {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid",
            };

            return requiredFields.All(dictionary.ContainsKey);
        }
        
        
        private List<Dictionary<string, string>> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day4input";
            var file = new StreamReader(filePath);
            
            var passportGroups = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>()
            };
            string line;
            
            while ((line = file.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    passportGroups.Add(new Dictionary<string, string>());
                    continue;
                }

                var splits = line.Split(" ");
                foreach (var keyValueString in splits)
                {
                    var keyValue = keyValueString.Split(":");
                    passportGroups.Last().Add(keyValue[0], keyValue[1]);
                }
            }

            file.Close();

            return passportGroups;
        }
    }
}