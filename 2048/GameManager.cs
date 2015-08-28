using System;

namespace _2048
{
    public class GameManager
    {
        private MatrixHelper _matrixHelper;
        public static GameStatusEnum GameStatue { get; set; }

        public GameManager()
        {
            _matrixHelper = new MatrixHelper();
        }

        public void ReStart()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    Matrix._cells[i, j] = 0;
                }
            }

            GameStatue = GameStatusEnum.Normal;
            Matrix._score = 0;
            _matrixHelper.AddNewTile();
            _matrixHelper.AddNewTile();
        }

        private bool IsWin()
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

        private bool IsFailed()
        {
            Func<bool> hasZero = () =>
                {
                    foreach (var a in Matrix._cells)
                    {
                        if (a == 0)
                        {
                            return true;
                        }
                    }
                    return false;
                };

            Func<bool> moveAble = () =>
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
            };

            return !hasZero() && !moveAble();
        }

        public void TryMove(DirectionEnum directionEnum)
        {
            if (_matrixHelper.Move(directionEnum))
            {
                if (IsWin())
                {
                    GameStatue = GameStatusEnum.Win;
                }
                else
                {
                    _matrixHelper.AddNewTile();
                }
            }
            else
            {
                if (IsFailed())
                {
                    GameStatue = GameStatusEnum.Failed;
                }
            }
        }
    }
}
