using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CybosAutoLogin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form1 = new Form1();
            form1.Visible = false;
            Application.Run(form1);            
        }
    }
}
