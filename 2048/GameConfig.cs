
namespace _2048
{
    public class GameConfig
    {
        public static int CellWidth
        {
            get
            {
                return 6;
            }
        }

        public static string BlanksInLineFront
        {
            get
            {
                return GetBlanks(BlanksCountInLineFront);
            }
        }

        public static string BlanksInColumnGap
        {
            get
            {
                return GetBlanks(BlanksCountInColumnGap);
            }
        }

        public static string GetBlanks(int count)
        {
            return string.Format("{0," + count + "}", " ");
        }

        public static int BlanksCountInColumnGap
        {
            get
            {
                return 2;
            }
        }

        public static int BlanksCountInLineFront
        {
            get
            {
                return 2;
            }
        }

        public static int WindowsWidth
        {
            get
            {
                return CellWidth * 4 + BlanksCountInColumnGap * 3 + BlanksCountInLineFront * 2;
            }
        }

        public static int WindowHeight
        {
            get
            {
                return 4 * 6-2;
            }
        }

        public static string[] ColorStrings = new string[] 
        {
            "#776e65", "#3c3a32", "#f9f6f2", "#bbada0", 
            "#eee4da", "#ede0c8", "#f2b179", "#f59563", 
            "#f67c5f", "#f65e3b", "#edcf72", "#edcc61", 
            "#edc850", "#edc53f", "#edc22e", "#3c3a32"
        };
    }
}
