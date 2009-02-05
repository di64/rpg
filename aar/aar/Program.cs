using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace aar
{
    static class Program
    {
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmRenderTarget());

            RenderManager rm = new RenderManager();
            rm.Start();


            while (rm.Running)
            {
                // game code goez 'ere

                Thread.Sleep(20);
            }
        }
    }
}
