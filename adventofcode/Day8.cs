using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day8
    {
        [Test]
        public void Solution1()
        {
            var instructions = Parse();
            
            var result = ExecuteInstructions(instructions);

            Assert.That(result.Accumulator, Is.EqualTo(1528));
        }

        [Test]
        public void Solution2()
        {
            var instructions = Parse();

            (int Accumulator, bool Valid) result = (0, false);
            foreach (var instruction in instructions)
            {
                ToggleInstruction(instruction);

                result = ExecuteInstructions(instructions);
                if (result.Valid)
                {
                    break;
                }
                    
                ToggleInstruction(instruction);
            }

            Assert.That(result.Accumulator, Is.EqualTo(640));
        }

        private (int Accumulator, bool Valid) ExecuteInstructions(List<Instruction> instructions)
        {
            var accumulator = 0;
            var visited = new HashSet<Instruction>();
            var currentLine = 0;

            while (currentLine < instructions.Count)
            {
                if (visited.Contains(instructions[currentLine])) return (accumulator, false);;

                var currentInstruction = instructions[currentLine];
                if (currentInstruction.Action == "nop")
                {
                    currentLine++;
                }

                if (currentInstruction.Action == "acc")
                {
                    accumulator += currentInstruction.Delta;
                    currentLine++;
                }

                if (currentInstruction.Action == "jmp")
                {
                    currentLine += currentInstruction.Delta;
                }

                visited.Add(currentInstruction);
            }

            return (accumulator, true);
        }

        private void ToggleInstruction(Instruction instruction)
        {
            switch (instruction.Action)
            {
                case "nop":
                    instruction.Action = "jmp";
                    return;
                case "jmp":
                    instruction.Action = "nop";
                    return;
                default:
                    return;
            }
        }

        private List<Instruction> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day8input";
            var file = new StreamReader(filePath);

            var instructions = new List<Instruction>();
            string line;

            while ((line = file.ReadLine()) != null)
            {
                var keyValue = line.Split(" ");
                var value = int.Parse(keyValue[1]);

                
                instructions.Add(new Instruction
                {
                    Action = keyValue[0],
                    Delta = value
                });

            }
            
            file.Close();
            
            return instructions;
        }

        private class Instruction
        {
            public string Action { get; set; }
            public int Delta { get; set; }
        }
    }
}