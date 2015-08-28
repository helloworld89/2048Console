
using Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace _2048
{
    public unsafe static class Matrix
    {
        public static readonly int[,] _cells = new int[4, 4];
        public static int _score;

        static Matrix()
        {
            Task t = new Task(() =>
            {
                fixed (int* p = Matrix._cells)
                {
                    using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
                    {
                        tmpEvent.WaitOne();
                    }
                }
            });
            t.Start();
        }

        public static void Reset()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Matrix._cells[i, j] = 0;
                }
            }
            Matrix._score = 0;
        }

        public static bool HasZero()
        {
            foreach (var a in Matrix._cells)
            {
                if (a == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Has2048()
        {
            foreach (var i in Matrix._cells)
            {
                if (i == 2048)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsMoveAble()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Matrix._cells[i, j] == Matrix._cells[i, j + 1] ||
                        Matrix._cells[j, i] == Matrix._cells[j + 1, i])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static void AddNewTile()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var emptyCells = GetEmptyCells();
            var emptyCell = random.Next(0, emptyCells.Count - 1);

            var value = random.Next(0, 100) < 90 ? 2 : 4;
            Matrix._cells[emptyCells[emptyCell].X, emptyCells[emptyCell].Y] = value;
        }

        private static List<Point> GetEmptyCells()
        {
            var emptyCells = new List<Point>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Matrix._cells[i, j] == 0)
                    {
                        emptyCells.Add(new Point(i, j));
                    }
                }
            }
            return emptyCells;
        }
    }
}
