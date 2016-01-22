using System.Collections.Generic;
using System.Drawing;

namespace _2048
{
    public static class MathHelper
    {
        public static IEnumerable<Point> GetSpiralArray(int width, int height)
        {
            int leftBorder = 0, upBorder = 0;
            int rightBorder = width - 1, downBorder = height - 1;
            int x = 0, y = 0;
            var directionEnum = DirectionEnum.Right;

            for (var i = 0; i < width * height; i++)
            {
                yield return new Point(x, y);

                switch (directionEnum)
                {
                    case DirectionEnum.Right:
                        {
                            if (x < rightBorder)
                            {
                                x++;
                            }
                            else
                            {
                                y++; upBorder++;
                                directionEnum = DirectionEnum.Down;
                            };
                        } break;
                    case DirectionEnum.Down:
                        {
                            if (y < downBorder)
                            {
                                y++;
                            }
                            else
                            {
                                x--; rightBorder--;
                                directionEnum = DirectionEnum.Left;
                            }
                        } break;
                    case DirectionEnum.Left:
                        {
                            if (x > leftBorder)
                            {
                                x--;
                            }
                            else
                            {
                                y--; downBorder--;
                                directionEnum = DirectionEnum.Up;
                            }
                        } break;

                    case DirectionEnum.Up:
                        {
                            if (y > upBorder)
                            {
                                y--;
                            }
                            else
                            {
                                x++; leftBorder++;
                                directionEnum = DirectionEnum.Right;
                            }
                        } break;
                }
            }
        }
    }
}