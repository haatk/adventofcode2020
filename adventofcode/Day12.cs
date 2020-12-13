using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace AdventOfCode
{
    public class Day12
    {
        [Test]
        public void Solution1()
        {
            var actions = Parse();

            var positionX = 0;
            var positionY = 0;
            var direction = Direction.East;
            
            foreach (var action in actions)
            {
                switch (action.Action)
                {
                    case 'N':
                        positionY -= action.Distance;
                        break;
                    case 'S':
                        positionY +=  action.Distance;
                        break;
                    case 'E':
                        positionX +=  action.Distance;
                        break;
                    case 'W':
                        positionX -= action.Distance;
                        break;
                    case 'L':
                        direction = (Direction) ((((int) direction) - ((action.Distance / 90) % 4) + 4) % 4);
                        break;
                    case 'R':
                        direction = (Direction) ((((int) direction) + ((action.Distance / 90) % 4) + 4) % 4);
                        break;
                    case 'F' when direction == Direction.East:
                        positionX +=  action.Distance;
                        break;
                    case 'F' when direction == Direction.North:
                        positionY -=  action.Distance;
                        break;
                    case 'F' when direction == Direction.West:
                        positionX -=  action.Distance;
                        break;
                    case 'F' when direction == Direction.South:
                        positionY +=  action.Distance;
                        break;
                }
            }
            
            Assert.That(Math.Abs(positionX) + Math.Abs(positionY), Is.EqualTo(1007));
        }

        [Test]
        public void Solution2()
        {
            var actions = Parse();

            var positionX = 0;
            var positionY = 0;
            var waypointPositionX = 10;
            var waypointPositionY = -1;
            
            foreach (var action in actions)
            {
                switch (action.Action)
                {
                    case 'N':
                        waypointPositionY -= action.Distance;
                        break;
                    case 'S':
                        waypointPositionY +=  action.Distance;
                        break;
                    case 'E':
                        waypointPositionX +=  action.Distance;
                        break;
                    case 'W':
                        waypointPositionX -= action.Distance;
                        break;
                    case 'L':
                        var (xL, yL) = Rotate(-action.Distance, waypointPositionX, waypointPositionY);
                        waypointPositionX = xL;
                        waypointPositionY = yL;
                        break;
                    case 'R':
                        var (xR, yR) = Rotate(action.Distance, waypointPositionX, waypointPositionY);
                        waypointPositionX = xR;
                        waypointPositionY = yR;
                        break;
                    case 'F':
                        positionX += waypointPositionX * action.Distance;
                        positionY += waypointPositionY * action.Distance;
                        break;
                }
            }
            
            Assert.That(Math.Abs(positionX) + Math.Abs(positionY), Is.EqualTo(41212));
        }

        private (int X, int Y) Rotate(int degrees, int waypointPositionX, int waypointPositionY)
        {
            var cos = Math.Cos(degrees * (Math.PI / 180));
            var sin = Math.Sin(degrees * (Math.PI / 180));
            var tempXL = (int) Math.Round(cos * waypointPositionX - sin * waypointPositionY);
            var y = (int) Math.Round(sin * waypointPositionX + cos * waypointPositionY);
            var x = tempXL;

            return (x, y);
        }

        private List<(char Action, int Distance)> Parse()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day12input";
            var file = new StreamReader(filePath);

            var actions = new List<(char Action, int Distance)>();
            string line;
            
            while ((line = file.ReadLine()) != null)
            {
                var action = line.First();
                var distance = int.Parse(line.Substring(1));
                actions.Add((action, distance));
            }
            
            file.Close();
            
            return actions;
        }
        
        private List<string> Parse2()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{dirName}/day12input";
            var file = new StreamReader(filePath);

            var actions = new List<string>();
            string line;
            
            while ((line = file.ReadLine()) != null)
            {
                actions.Add(line);
            }
            
            file.Close();
            
            return actions;
        }

        private enum Direction
        {
            East,
            South,
            West,
            North
        }
    }
}