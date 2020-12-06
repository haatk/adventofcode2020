using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day5
    {
        [Test]
        public void Solution1()
        {
            var seats = Parse();
            
            var highest = seats.Select(seat => GetSeatId(seat)).Prepend(0).Max();

            Assert.That(highest, Is.EqualTo(880));
        }

        [Test]
        public void Solution2()
        {
            var seats = Parse();
            var dict = new Dictionary<byte, Dictionary<byte, bool>>();
            foreach (var seat in seats)
            {
                var rowColumnTuple = GetRowColumnTuple(seat);

                if (!dict.ContainsKey(rowColumnTuple.Row))
                {
                    dict.Add(rowColumnTuple.Row, new Dictionary<byte, bool>());
                }

                if (!dict[rowColumnTuple.Row].ContainsKey(rowColumnTuple.Column))
                {
                    dict[rowColumnTuple.Row].Add(rowColumnTuple.Column, true);
                }
            }

            var rows = dict.Where(g => g.Value.Count != 8).ToArray();
            var middleRow = rows.Skip(1).Reverse().Skip(1).Single();
            var middleRowColumns = middleRow.Value;
            byte columnIndex = 0;
            
            for (byte i = 0; i < middleRowColumns.Count; i++)
            {
                if (!middleRowColumns.ContainsKey(i))
                {
                    columnIndex = i;
                    break;
                }
            }

            var answer = middleRow.Key * 8 + columnIndex;
            Assert.That(answer, Is.EqualTo(731));
        }

        private (byte Row, byte Column) GetRowColumnTuple(string seat)
        {
            var rowChars = seat.Take(7).ToArray();
            var columnChars = seat.Substring(seat.Length - 3).ToArray();

            var row = GetRow(rowChars);
            var column = GetColumn(columnChars);
            
            return (row, column);
        }

        private int GetSeatId(string seat)
        {
            var rowColumnTuple = GetRowColumnTuple(seat);

            return rowColumnTuple.Row * 8 + rowColumnTuple.Column;
        }

        private byte GetColumn(char[] columnChars)
        {
            bool[] bits = columnChars.Select(c => c != 'L').ToArray();
            BitArray bitArray =
                new BitArray(new[] {false, false, false, false, false}.Concat(bits).Reverse().ToArray());
            byte b = ConvertToByte(bitArray);
            return b;
        }

        private byte GetRow(char[] rowChars)
        {
            bool[] bits = rowChars.Select(c => c != 'F').ToArray();
            BitArray bitArray = new BitArray(new[] {false}.Concat(bits).Reverse().ToArray());
            byte b = ConvertToByte(bitArray);
            return b;
        }

        byte ConvertToByte(BitArray bits)
        {
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }

        private List<string> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day5input";
            var file = new StreamReader(filePath);
            
            var seats = new List<string>();
            string line;
            
            while ((line = file.ReadLine()) != null)
            {
                seats.Add(line);
            }

            file.Close();

            return seats;
        }
    }
}