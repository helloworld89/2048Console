namespace _2048
{
    public class GameManager
    {
        public static GameStatusEnum GameStatue { get; set; }

        public void ReStart()
        {
            Matrix.Reset();
            Matrix.AddNewTile();
            Matrix.AddNewTile();
            GameStatue = GameStatusEnum.Normal;
        }

        public void TryMove(DirectionEnum directionEnum)
        {
            if (MatrixHelper.Move(directionEnum))
            {
                if (IsWin())
                {
                    GameStatue = GameStatusEnum.Win;
                }
                else
                {
                    Matrix.AddNewTile();
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

        private bool IsWin()
        {
            return Matrix.Has2048();
        }

        private bool IsFailed()
        {
            return !Matrix.HasZero() && !Matrix.IsMoveAble();
        }
    }
}