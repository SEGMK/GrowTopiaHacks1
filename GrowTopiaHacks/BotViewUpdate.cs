using IronOcr;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GrowTopiaHacks
{
    class BotViewUpdate
    {
        // player value - 5
        //farmable block value - 1
        //block value - 2
        //background value - 0
        private int[,] PlayerPositionUpdate(Bitmap screen, string userName, int[,] botView)
        {
            var Result = new IronTesseract().Read(screen);            
            foreach (var i in Result.Blocks)
            {
                if (i.Text == userName)
                {
                    (int, int) leftUpperCorner = (i.Location.X, i.Location.Y);
                    (int, int) rightDownCorner = (i.Location.Right, i.Location.Bottom);
                    (int, int) middlePoint = MiddleOfLineSegment(leftUpperCorner, rightDownCorner);
                    int posXOfPlayer = ArithmeticSequencePositionFinderFloorRounding(middlePoint.Item1, 32, 64);
                    int posYOfPlayer = ArithmeticSequencePositionFinderFloorRounding(middlePoint.Item2, 28, 64) + 1;
                    botView[posXOfPlayer, posYOfPlayer] = 5;
                }
            }
            return botView;
        }
        public int[,] DataToTableInterpreter(List<Dictionary<int, int>> EntryData, int[,] botView, string userName, Bitmap screen) //Dictionary<2000, posY>
        {            
            int posXOfTable = -1;
            int posYOfTable = -1;
            foreach (var i in EntryData)
            {
                foreach (var j in i)
                {
                    // its working cuz first iterated value is value with key 2000
                    if (j.Key != 2000)
                    {
                        posXOfTable = ArithmeticSequencePositionFinderFloorRounding(j.Key, 32, 64);
                    }
                    else
                    {
                        posYOfTable = ArithmeticSequencePositionFinderFloorRounding(j.Value, 28, 64);
                    }
                    try
                    {
                        botView[posXOfTable, posYOfTable] = j.Value;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        continue;
                    }
                }
            }
            return PlayerPositionUpdate(screen, userName, botView);
        }
        private int ArithmeticSequencePositionFinderFloorRounding(float an, int a1, int d)
        {            
            an += d + a1;
            an = an / d;
            return (int)Math.Floor(an);
        }
        private (int, int) MiddleOfLineSegment((int,int) A, (int, int) B)
        {
            (int, int) middlePoint = ((A.Item1 +B.Item1) / 2, (A.Item2 + B.Item2) / 2);
            return middlePoint;
        }
    }
}
