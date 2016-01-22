using System;
using System.Collections.Generic;
using System.Drawing;

namespace _2048
{
    public class OutPutHelper
    {
        public static void RefreshScreen()
        {
            //reset color and clear screen
            OutPut(backgroundColor: GameConfig.ColorStrings[0]);
            Console.Clear();

            ShowPartition(() => OutPutPromptInfo("Reset: \"F5\", Move: \" ←↑→↓\""));

            ShowPartition(ShowScore);

            ShowPartition(ShowMatrix);
        }

        private static void ShowScore()
        {
            var _gameStatusPromptInfoDic = new Dictionary<GameStatusEnum, string>()
            {
                 {GameStatusEnum.Win,"You have win"},
                 {GameStatusEnum.Failed,"Cann't move"},
                 {GameStatusEnum.Normal,string.Empty},
            };

            var promptInfo = _gameStatusPromptInfoDic[GameManager.GameStatue];

            //output Score
            for (int i = 0; i < 2; i++)
            {
                var scoreWidth = GameConfig.CellWidth + 4;

                Console.WriteLine();
                OutPutPromptInfo(i == 0 ? promptInfo : "");

                OutPut(GameConfig.GetBlanks(GameConfig.WindowsWidth - scoreWidth - GameConfig.BlanksCountInLineFront * 2 - (i == 0 ? promptInfo.Length : 0)),
                    backgroundColor: GameConfig.ColorStrings[0]);

                var centeredText = GetCenteredText(scoreWidth, i == 0 ? "Score:" : Matrix._score.ToString());
                var foregroundColor = GameConfig.ColorStrings[2];
                var backgroundColor = GameConfig.ColorStrings[3];
                OutPut(centeredText, foregroundColor, backgroundColor);
            }
        }

        private static void ShowMatrix()
        {
            for (var y = 0; y < 4; y++)
            {
                Action outPutLineInMatrix = () =>
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine();
                        OutPut(GameConfig.BlanksInLineFront, backgroundColor: GameConfig.ColorStrings[0]);
                        for (var x = 0; x < 4; x++)
                        {
                            OutPutCell(Matrix._cells[x, y], i);

                            if (x != 3)
                            {
                                OutPut(GameConfig.BlanksInColumnGap, backgroundColor: GameConfig.ColorStrings[0]);
                            }
                        }
                    }
                };
                outPutLineInMatrix();

                Console.WriteLine();
            }
        }

        private static void ShowPartition(Action action)
        {
            Console.WriteLine();
            action();
        }

        private static string GetCenteredText(int width, string text)
        {
            Func<int, string, string> formatFunc = (margin, textToFormat) =>
            {
                var formatString = "{0," + margin + "}";
                return string.Format(formatString, textToFormat);
            };

            var marginLeft = (width - text.Length) / 2;
            return formatFunc(width, formatFunc(marginLeft - width, text));
        }

        private static void OutPutPromptInfo(string s)
        {
            s = GameConfig.BlanksInLineFront + s;
            OutPut(text: s, foregroundColor: GameConfig.ColorStrings[13]);
        }

        private static void OutPutCell(int value, int i, bool showZero = false)
        {
            var centeredText = GetCenteredText(GameConfig.CellWidth, (value != 0 || showZero == true) && i == 1 ? value.ToString() : " ");
            var colorIndex = value == 0 ? 0 : Math.Log(value, 2);

            var foregroundColor = GameConfig.ColorStrings[colorIndex < 3 ? 1 : 2];
            var backgroundColor = GameConfig.ColorStrings[(int)colorIndex + 3];
            OutPut(centeredText, foregroundColor, backgroundColor);
        }

        private static void OutPut(string text = null, string foregroundColor = null, string backgroundColor = null)
        {
            Func<string, Color> getColor = (s) =>
            {
                var colorConverter = new ColorConverter();
                return (Color)colorConverter.ConvertFromString(s);
            };

            Predicate<string> notNullofEmpty = x => !string.IsNullOrEmpty(x);

            if (notNullofEmpty(backgroundColor))
            {
                ColorSetter.BackgroundColor = getColor(backgroundColor);
            }
            if (notNullofEmpty(foregroundColor))
            {
                ColorSetter.ForegroundColor = getColor(foregroundColor);
            }
            if (notNullofEmpty(text))
            {
                Console.Write(text);
            }

            ColorSetter.BackgroundColor = getColor(GameConfig.ColorStrings[0]);
        }
    }
}