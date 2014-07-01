using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CybosAutoLogin
{
    class Ut
    {
        public static void Log(string text)
        {   
            Console.Write(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "\t" + text + "\n");
        }
    }
}
