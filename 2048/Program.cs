using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace _2048
{
    class Program
    {
        static GameManager _gameManager = new GameManager();

        static Dictionary<ConsoleKey, DirectionEnum> _consoleKeyDic = new Dictionary<ConsoleKey, DirectionEnum>()
        {
            {ConsoleKey.UpArrow,DirectionEnum.Up},
            {ConsoleKey.DownArrow,DirectionEnum.Down},
            {ConsoleKey.LeftArrow,DirectionEnum.Left},
            {ConsoleKey.RightArrow,DirectionEnum.Right}
        };

        static void Main(string[] args)
        {
            Console.Title = "2048";
            Console.BufferWidth = Console.WindowWidth = GameConfig.WindowsWidth;
            Console.BufferHeight = Console.WindowHeight = GameConfig.WindowHeight;
            Console.CursorVisible = false;

            _gameManager.ReStart();

            while (true)
            {
                OutPutHelper.RefreshScreen();

                var key = Console.ReadKey().Key;
                if (_consoleKeyDic.Keys.Contains(key))
                {
                    Move(_consoleKeyDic[key]);
                }
                else if (key == ConsoleKey.F5)
                {
                    _gameManager.ReStart();
                }
            }
        }

        private static void Move(DirectionEnum directionEnum)
        {
            if (GameManager.GameStatue == GameStatusEnum.Normal)
            {
                _gameManager.TryMove(directionEnum);
            }
            else if (GameManager.GameStatue == GameStatusEnum.Failed)
            {
                GameOver();
                _gameManager.ReStart();
            }
        }

        private static void GameOver()
        {
            foreach (var point in MathHelper.GetSpiralArray(4, 4))
            {
                Thread.Sleep(100);
                Matrix._cells[point.X, point.Y] = 0;
                OutPutHelper.RefreshScreen();
            }
        }
    }
}