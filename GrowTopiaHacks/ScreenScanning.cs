using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;

namespace GrowTopiaHacks
{    
    class ScreenScanning
    {
        List<List<int>> FarmableBlocksList = new List<List<int>>();
        List<int> BgBlocksList = new List<int>();
        public Bitmap Screen;
        private ScreenScanning()
        {            
            
        }
        private void CopyingOutsideData()
        {
            using (StreamReader reader = new StreamReader("farmableBlocks.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var firstSeparation = line.Split(';');
                    for (int i = 0; i < 5; i++)
                    {
                        var secondSeparation = firstSeparation[i].Split(',');
                        int[] pixelOutPut = Array.ConvertAll(secondSeparation, x => int.Parse(x));
                        FarmableBlocksList.Add(pixelOutPut.ToList());
                    }
                }
            }
            using (StreamReader reader = new StreamReader("bgBlocks.txt"))
            {
                var line = reader.ReadLine();
                var pixelInPut = line.Split(',');
                BgBlocksList = Array.ConvertAll(pixelInPut, x => int.Parse(x)).ToList();
            }
        }
        public static List<ScreenScanning> MultipleCreation(int numberOfCreationOfObjects)
        {
            List<ScreenScanning> Creations = new List<ScreenScanning>();
            ScreenScanning classObject = new ScreenScanning();
            classObject.CopyingOutsideData();
            for (int i = 1; i <= numberOfCreationOfObjects; i++)
            {
                ScreenScanning deepCopy = new ScreenScanning();
                deepCopy.FarmableBlocksList = new List<List<int>>(classObject.FarmableBlocksList);
                deepCopy.BgBlocksList = new List<int>(classObject.BgBlocksList);
                Creations.Add(deepCopy);
            }
            return Creations;
        }
        public Dictionary<int, int> XAxysScan(int posY)
        {            
            Dictionary<int, int> partOfMainTable = new Dictionary<int, int>();
            partOfMainTable.Add(2000, posY); //used in BotViewUpdate in DataToTableInterpreter
            for (int i = 0; i < 1920; i++)
            {
                if (IsFarmable(posY, i))
                {
                    partOfMainTable.Add(i, 1);
                }
                else if (IsBackground(i, posY))
                {
                    partOfMainTable.Add(i, 0);
                }
                else
                {
                    partOfMainTable.Add(i, 2);
                }
            }
            return partOfMainTable;
        }
        private bool IsFarmable(int posY, int posX)
        {
            int iteratedPosX = posX;
            int k;
            Color farmableBlockColor;
            for (int i = 0; i < FarmableBlocksList.Count;)
            {
                i++;
                k = i - 1;
                farmableBlockColor = Color.FromArgb(FarmableBlocksList[k][0], FarmableBlocksList[k][1], FarmableBlocksList[k][2]);
                if (Screen.GetPixel(posX, posY) != farmableBlockColor)
                {
                    while (!(i % 5 == 0))
                    {
                        i++;
                    }
                    iteratedPosX = posX;
                    continue;
                }
                iteratedPosX++;
                if (posX + 5 == iteratedPosX)
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsBackground(int posX, int posY)
        {
            Color bgColor;
            for (int i = 0; i < BgBlocksList.Count; i += 3)
            {
                bgColor = Color.FromArgb(BgBlocksList[i], BgBlocksList[i + 1], BgBlocksList[i + 2]);
                if (Screen.GetPixel(posX, posY) == bgColor)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
