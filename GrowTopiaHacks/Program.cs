using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace GrowTopiaHacks
{
    class Program
    {
        public const int BlocksOnXAxisOnScreen = 29;
        public const int BlocksOnYAxisOnScreen = 16;
        public static string UserName = "test";        
        public static List<int> PosYOfRowsToScan = new List<int>() {38, 102, 166, 130, 194, 358, 486, 550, 614, 678, 742, 806, 870, 934, 998, 1062};        
        public static Process GrowTopiaApp = new Process();
        public static Bitmap Screen;
        public static int[,] RepresentationOfBotView = new int[BlocksOnXAxisOnScreen, BlocksOnYAxisOnScreen];
        public static List<ScreenScanning> ListOfScreenScaningClasses = new List<ScreenScanning>();
        public static Mutex Mut = new Mutex();
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your user name");
            UserName = Console.ReadLine();
            GrowTopiaApp.StartInfo.UseShellExecute = true;
            GrowTopiaApp.StartInfo.FileName = "D:\\Users\\SEGMK\\AppData\\Local\\Growtopia\\Growtopia.exe";
            //GrowTopiaApp.Start();
            Console.WriteLine("Press any key if you enter your world and you are ready for farming");
            Console.ReadKey();
            Console.Clear();
            ScreenReader sr = new ScreenReader();                                    
            BotViewUpdate bv = new BotViewUpdate();
            ScreenReader(sr);
            ListOfScreenScaningClasses = ScreenScanning.MultipleCreation(16, Screen);
            while (true)
            {
                ScreenReader(sr);
                foreach (var i in ListOfScreenScaningClasses)
                {
                    i.Screen = Screen;
                }
                List<Dictionary<int, int>> RunTasksPassingVariable = new List<Dictionary<int, int>>(RunTasks());
                RepresentationOfBotView = bv.DataToTableInterpreter(RunTasksPassingVariable, RepresentationOfBotView, UserName, Screen); // Dictionary<666, posY of decoded row>
                TestFuction(RepresentationOfBotView); //test
            }            
        }
        public static ConcurrentBag<Dictionary<int, int>> RunTasks()
        {            
            var listOfTasks = new List<Task>();            
            ConcurrentBag<Dictionary<int, int>> PosXOfBlocksAndMeaningOfIt = new ConcurrentBag<Dictionary<int, int>>();
            for (var i = 0; i < BlocksOnYAxisOnScreen - 1; i++)
            {                
                var t = new Task(() =>
                {                    
                    PosXOfBlocksAndMeaningOfIt.Add(ListOfScreenScaningClasses[i].XAxysScan(PosYOfRowsToScan[i])); // u re here
                });
                t.Start();
                listOfTasks.Add(t);
            }
            Task.WaitAll(listOfTasks.ToArray());
            return PosXOfBlocksAndMeaningOfIt;
        }
        public static void TestFuction(int[,] table)
        {
            for (int k = 0; k < table.GetLength(0); k++)
            {
                for (int l = 0; l < table.GetLength(1); l++)
                {
                    Console.Write(table[k,l]);
                }
                Console.WriteLine();
            }
        }
        public static void ScreenReader(ScreenReader sr)
        {            
            Screen = sr.InputScreen(Screen);            
        }
    }
}