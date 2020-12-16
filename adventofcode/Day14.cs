using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day14
    {
        private Regex _memRegex = new Regex(@"^mem\[(?'index'\d*)\] = (?'value'\d*)$");
        
        [Test]
        public void Solution1()
        {
            var lines = Parse();

            var currentMask = new List<(int bitIndex, bool value)>();
            var numberDictionary = new Dictionary<int, ulong>();
            foreach (var line in lines)
            {
                if (line.StartsWith("mask = "))
                {
                    currentMask = GetMaskBits(line).Where(l => l.value.HasValue).Select(l => (l.bitIndex, l.value.Value)).ToList();
                }
                else
                {
                    var match = _memRegex.Match(line);
                    var index = int.Parse(match.Groups["index"].Value);
                    var value = ulong.Parse(match.Groups["value"].Value);
                    var bits = SetBits(value, currentMask);
                    
                    if (numberDictionary.ContainsKey(index))
                    {
                        numberDictionary[index] = bits;
                    }
                    else
                    {
                        numberDictionary.Add(index, bits);    
                    }
                }
            }

            var total = numberDictionary.Select(keyValue => keyValue.Value).Aggregate((x, y) => y + x);
            
            Assert.That(total, Is.EqualTo(9615006043476));
        }

        private List<(int bitIndex, bool? value)> GetMaskBits(string line)
        {
            var maskString = line.Substring(7);
            var maskBits = new List<(int bitIndex, bool? value)>();
            for (var i = 0; i < maskString.Length; i++)
            {
                var maskChar = maskString[i];
                var index = 35 - i;
                switch (maskChar)
                {
                    case '1':
                        maskBits.Add((index, true));
                        break;
                    case '0':
                        maskBits.Add((index, false));
                        break;
                    default:
                        maskBits.Add((index, null));
                        break;
                }
            }

            return maskBits;
        }

        private ulong SetBits(ulong value, List<(int bitIndex, bool value)> maskBits)
        {
            var bytes = BitConverter.GetBytes(value);
            var bitArray = new BitArray(bytes);

            foreach (var maskBit in maskBits)
            {
                bitArray[maskBit.bitIndex] = maskBit.value;
            }
            
            var bytesOutput = new byte[8];
            bitArray.CopyTo(bytesOutput,0);

            var result = BitConverter.ToUInt64(bytesOutput);
            
            return result;
        }
        
        private List<string> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day14input";
            var file = new StreamReader(filePath);

            var lines = new List<string>();
            string line;

            var lineIndex = 0;
            
            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }
            
            file.Close();
            
            return lines;
        }
    }
}