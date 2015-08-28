using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _2048
{
    public static unsafe class MatrixHelper
    {
        private static Dictionary<DirectionEnum, int*[,]> _directionDic = new Dictionary<DirectionEnum, int*[,]>()
        {
            {DirectionEnum.Up, new int*[4, 4]},
            {DirectionEnum.Down, new int*[4, 4]},
            {DirectionEnum.Left, new int*[4, 4]},
            {DirectionEnum.Right, new int*[4, 4]}
        };

        static MatrixHelper()
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    fixed (int* intPoint = &Matrix._cells[i, j])
                    {
                        _directionDic[DirectionEnum.Up][i, j] = intPoint;
                        _directionDic[DirectionEnum.Down][i, 3 - j] = intPoint;
                        _directionDic[DirectionEnum.Left][j, i] = intPoint;
                        _directionDic[DirectionEnum.Right][3 - j, 3 - i] = intPoint;
                    }
                }
            }
        }

        public static bool Move(DirectionEnum directionEnum)
        {
            return MoveUp(_directionDic[directionEnum]);
        }

        private static bool MoveUp(int*[,] intP)
        {
            var moved = false;
            Parallel.For(0, 4, (x) =>
            {
                for (var y = 0; y < 3; y++)
                {
                    //jump empty line
                    var t = 0;
                    for (var i = y; i < 4; i++)
                    {
                        t += *intP[x, i];
                    }
                    if (t == 0)
                    {
                        break;
                    }

                    //remove 0 at first
                    if (*intP[x, y] == 0)
                    {
                        for (int i = y; i < 3; i++)
                        {
                            *intP[x, i] = *intP[x, i + 1];
                        }
                        y--;
                        *intP[x, 3] = 0;
                        moved = true;
                    }
                    else
                    {
                        for (var i = y + 1; i < 4; i++)
                        {
                            if (*intP[x, i] == 0) continue;
                            if (*intP[x, y] != *intP[x, i]) break;
                            if (*intP[x, y] == *intP[x, i])
                            {
                                *intP[x, y] += *intP[x, y];
                                *intP[x, i] = 0;
                                Interlocked.Add(ref Matrix._score, *intP[x, y]);
                                moved = true;
                                break;
                            }
                        }
                    }
                }
            });
            return moved;
        }
    }
}
