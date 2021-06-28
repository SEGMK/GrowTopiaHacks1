using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GrowTopiaHacks
{
    class ScreenReader
    {
        public Bitmap InputScreen(Bitmap Screen)
        {                        
            Screen = new Bitmap(1920, 1080);
            Graphics gr = Graphics.FromImage(Screen);
            gr.CopyFromScreen(0, 0, 0, 0, Screen.Size);            
            return Screen;
        }
    }
}
