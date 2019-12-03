﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2019
{
    public class Day3
    {
        struct Step
        {
            public readonly IntPair Delta;
            public readonly int Count;

            public Step(IntPair delta, int count)
            {
                Delta = delta;
                Count = count;
            }

            public static Step Parse(string str)
            {
                IntPair delta = str[0] switch
                {
                    'U' => IntPair.Up,
                    'D' => IntPair.Down,
                    'L' => IntPair.Left,
                    'R' => IntPair.Right,
                    _ => throw new InvalidOperationException("invalid direction")
                };

                int count = int.Parse(str.Substring(1));

                return new Step(delta, count);
            }
        }

        private Dictionary<IntPair, (int mask, int totalSteps)> _map;

        public Day3()
        {
            Step[][] steps = File.ReadAllLines("Inputs/Day3.txt").Select(l => l.Split(',').Select(Step.Parse).ToArray()).ToArray();

            _map = new Dictionary<IntPair, (int, int)>();

            for (int i = 0; i < steps.Length; i++)
            {
                DoSteps(_map, steps[i], i);
            }
        }

        [Fact]
        public void Part1()
        {
            int answer = _map.Min(kvp => kvp.Value.mask == 3 ? Math.Abs(kvp.Key.X) + Math.Abs(kvp.Key.Y) : int.MaxValue);
            Assert.Equal(248, answer);
        }

        [Fact]
        public void Part2()
        {
            int answer = _map.Min(kvp => kvp.Value.mask == 3 ? kvp.Value.totalSteps : int.MaxValue);
            Assert.Equal(28580, answer);
        }

        private void DoSteps(Dictionary<IntPair, (int mask, int totalSteps)> map, Step[] steps, int id)
        {
            IntPair current = new IntPair(0, 0);
            int totalSteps = 0;
            foreach (Step step in steps)
            {
                for (int i = 0; i < step.Count; i++)
                {
                    current += step.Delta;
                    totalSteps++;

                    (int mask, int totalStesp) state;
                    if (!map.TryGetValue(current, out state))
                    {
                        map.Add(current, (1 << id, totalSteps));
                    }
                    else
                    {
                        if ((map[current].mask & (1 << id)) == 0)
                        {
                            map[current] = (map[current].mask | (1 << id), map[current].totalSteps + totalSteps);
                        }
                    }

                }
            }
        }
    }
}
